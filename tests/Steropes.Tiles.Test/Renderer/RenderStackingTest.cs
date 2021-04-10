using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Plotter.Operations.Builder;
using Steropes.Tiles.Renderer;
using System;

namespace Steropes.Tiles.Test.Renderer
{
    class RenderCallbackBase<TTile, TContext> : IRenderCallback<TTile, TContext>
    {
        public void StartDrawing()
        {
            throw new NotImplementedException();
        }

        public void StartLine(int logicalLine, ContinuousViewportCoordinates screen)
        {
            throw new NotImplementedException();
        }

        public void Draw(TTile tile, TContext context, SpritePosition pos, ContinuousViewportCoordinates screenLocation)
        {
            throw new NotImplementedException();
        }

        public void EndLine(int logicalLine, ContinuousViewportCoordinates screen)
        {
            throw new NotImplementedException();
        }

        public void FinishedDrawing()
        {
            throw new NotImplementedException();
        }
    }

    public class RenderStackingTest
    {
        [Test]
        public void RenderSetUp_With_Cache()
        {
            var rc = new GameRenderingConfig(RenderType.Grid);
            var renderer = new RendererSubstitute<MatchedTile, MatchedContext>();

            var matcher = Substitute.For<ITileMatcher<MatchedTile, MatchedContext>>();
            var plot = PlotOperations.FromContext(rc).Create(matcher).WithCache().BuildUnsafe();

            Assert.IsTrue(CachingPlotOperation<MatchedTile, MatchedContext>.IsRecordingRenderer(plot.ActiveRenderer));

            plot.Renderer = renderer;

            Assert.IsTrue(CachingPlotOperation<MatchedTile, MatchedContext>.IsRecordingRenderer(plot.ActiveRenderer));
            Assert.AreEqual(plot.Renderer, renderer);
        }

        [Test]
        public void RenderAccess_With_Conversion()
        {
            var rc = new GameRenderingConfig(RenderType.Grid);
            var renderer = new RendererSubstitute<RenderedTile, RenderedContext>();
            var conv = new ConverterProxy();
            var matcher = Substitute.For<ITileMatcher<MatchedTile, MatchedContext>>();

            var sourceFactory = PlotOperations.FromContext(rc).Create(matcher);
            var plot = sourceFactory.WithConversion(conv).BuildUnsafe();

            var sourcePo = sourceFactory.BuildUnsafe();
            sourcePo.Renderer.Should().Be(conv);
            sourcePo.ActiveRenderer.Should().Be(conv);
            plot.ActiveRenderer.Should().BeNull();

            plot.Renderer = renderer;
            Assert.AreEqual(plot.Renderer, renderer);
            Assert.AreEqual(plot.ActiveRenderer, renderer);
            sourcePo.Renderer.Should().Be(conv);
            sourcePo.ActiveRenderer.Should().Be(conv);
        }

        [Test]
        public void RenderAccess_With_Conversion_And_Cache()
        {
            var rc = new GameRenderingConfig(RenderType.Grid);
            var renderer = new RendererSubstitute<RenderedTile, RenderedContext>();
            var conv = new ConverterProxy();
            var matcher = Substitute.For<ITileMatcher<MatchedTile, MatchedContext>>();

            var sourceFactory = PlotOperations.FromContext(rc).Create(matcher);
            var plotFactory = sourceFactory.WithConversion(conv);
            var cached = plotFactory.WithCache().BuildUnsafe();

            var sourcePo = sourceFactory.BuildUnsafe();
            var plot = plotFactory.BuildUnsafe();

            sourcePo.Renderer.Should().Be(conv);
            sourcePo.ActiveRenderer.Should().Be(conv);
            plot.ActiveRenderer.Should().Be(cached.ActiveRenderer);

            cached.Renderer = renderer;
            Assert.AreEqual(plot.Renderer, cached.ActiveRenderer);
            Assert.AreEqual(plot.ActiveRenderer, cached.ActiveRenderer);
            sourcePo.Renderer.Should().Be(conv);
            sourcePo.ActiveRenderer.Should().Be(conv);
        }

        [Test]
        public void RenderAccess_With_Conversion_Cache_And_ViewPort()
        {
            var rc = new GameRenderingConfig(RenderType.Grid);
            var renderer = new RendererSubstitute<RenderedTile, RenderedContext>();
            var conv = new ConverterProxy();
            var matcher = Substitute.For<ITileMatcher<MatchedTile, MatchedContext>>();

            var sourceFactory = PlotOperations.FromContext(rc).Create(matcher);
            var plotFactory = sourceFactory.WithConversion(conv);
            var cachedFactory = plotFactory.WithCache();
            var vp = cachedFactory.ForViewport().Build();

            var sourcePo = sourceFactory.BuildUnsafe();
            var plot = plotFactory.BuildUnsafe();
            var cached = cachedFactory.BuildUnsafe();

            sourcePo.Renderer.Should().Be(conv);
            sourcePo.ActiveRenderer.Should().Be(conv);
            plot.ActiveRenderer.Should().Be(cached.ActiveRenderer);

            vp.Renderer = renderer;
            Assert.AreEqual(plot.Renderer, cached.ActiveRenderer);
            Assert.AreEqual(plot.ActiveRenderer, cached.ActiveRenderer);
            sourcePo.Renderer.Should().Be(conv);
            sourcePo.ActiveRenderer.Should().Be(conv);

            vp.ActiveRenderer.Should().BeAssignableTo<ViewportRenderer<RenderedTile, RenderedContext>>();
        }

        [Test]
        public void RenderAccess_With_Cache_ViewPort_then_Conversion()
        {
            var rc = new GameRenderingConfig(RenderType.Grid);
            var renderer = new RendererSubstitute<RenderedTile, RenderedContext>();
            var conv = new ConverterProxy();
            var matcher = Substitute.For<ITileMatcher<MatchedTile, MatchedContext>>();

            var sourceFactory = PlotOperations.FromContext(rc).Create(matcher);
            var cachedFactory = sourceFactory.WithCache();
            var vpFactory = cachedFactory.ForViewport();
            var convRend = vpFactory.WithConversion(conv).Build();

            var sourcePo = sourceFactory.BuildUnsafe();
            var cached = cachedFactory.BuildUnsafe();
            var vp = vpFactory.Build();

            sourcePo.Renderer.Should().Be(cached.ActiveRenderer);
            sourcePo.ActiveRenderer.Should().Be(cached.ActiveRenderer);

            convRend.Renderer = renderer;

            Assert.AreEqual(vp.ActiveRenderer, cached.Renderer);
            Assert.AreEqual(vp.Renderer, conv);

            sourcePo.Renderer.Should().Be(cached.ActiveRenderer);
            sourcePo.ActiveRenderer.Should().Be(cached.ActiveRenderer);

            vp.ActiveRenderer.Should().BeAssignableTo<ViewportRenderer<MatchedTile, MatchedContext>>();
        }

        [Test]
        public void RenderAccess_With_Cache_ViewPort()
        {
            var rc = new GameRenderingConfig(RenderType.Grid);
            var renderer = new RendererSubstitute<RenderedTile, RenderedContext>();
            var conv = new ConverterProxy();
            var matcher = Substitute.For<ITileMatcher<RenderedTile, RenderedContext>>();

            var cache =
                PlotOperations.FromContext(rc)
                              .Create(matcher)
                              .WithCache()
                              .ForViewport()
                              .WithRenderer(renderer)
                              .Build();

            var s = cache.ActiveRenderer.Should().BeAssignableTo<ViewportRenderer<RenderedTile, RenderedContext>>().Subject;
            //s.RenderTarget.Should().BeAssignableTo<ViewportRenderer<RenderedTile, RenderedContext>>();
        }

        /// <summary>
        ///     Empty implementation that provides better debugging experience than those
        ///     horrible Castle.Proxy objects with their meaningless properties.
        /// </summary>
        /// <typeparam name="TTile"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        class RendererSubstitute<TTile, TContext> : RenderCallbackBase<TTile, TContext>
        {
            public override string ToString()
            {
                return $"{GetType()}";
            }
        }

        public class RenderedTile
        { }

        public class RenderedContext
        { }

        public class MatchedTile
        { }

        public class MatchedContext
        { }

        class ConverterProxy : RenderCallbackBase<MatchedTile, MatchedContext>,
                               IRenderCallbackFilter<MatchedTile, MatchedContext, RenderedTile, RenderedContext>
        {
            public IRenderCallback<RenderedTile, RenderedContext> RenderTarget { get; set; }

            public override string ToString()
            {
                return $"Converter({GetType()}), {nameof(RenderTarget)}: {RenderTarget}";
            }
        }
    }
}
