using Avalonia.Media;
using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.TemplateGen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGenerator.Test.Model
{
    public class TileFileWriterTest
    {
        static readonly XNamespace Namespace = "http://www.steropes-ui.org/namespaces/tiles/1.0";

        [Test]
        public void SavingEmptyTextureFileMustNotCrash()
        {
            var f = new TextureSetFile();
            var root = TextureSetFileWriter.GenerateRoot(f);

            root.Name.Should().Be(Namespace + "tileset");
            root.Should().HaveAttribute("width", "0");
            root.Should().HaveAttribute("height", "0");
            root.Should().HaveAttribute("type", "Grid");
            root.Should().HaveElement(Namespace + "metadata");
        }

        [Test]
        public void ValidateEmptyTextureFileMetadata()
        {
            var f = new TextureSetFile();
            var root = TextureSetFileWriter.GenerateRoot(f);
            var md = root.Element(Namespace + "metadata");
            md.HasAttributes.Should().Be(false);
            md.HasElements.Should().Be(false);
        }

        [Test]
        public void ValidateTextureFileProperties()
        {
            var f = new TextureSetFile
            {
                Properties = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
                {
                    {"pname1", "pvalue1"},
                    {"pname2", "pvalue2"}
                })
            };
            var root = TextureSetFileWriter.GenerateRoot(f);
            var md = root.Element(Namespace + "metadata");
            md.HasAttributes.Should().Be(false);

            var elements = md.Elements(Namespace + "property").ToList();
            elements.Should().HaveCount(2);
            elements[0].Should().HaveAttribute("name", "pname1").And.HaveValue("pvalue1");
            elements[1].Should().HaveAttribute("name", "pname2").And.HaveValue("pvalue2");
        }

        void SetUpMetaData(FormattingMetaData m)
        {
            m.BackgroundColor = Colors.Black;
            m.TextColor = null;
            m.BorderColor = Color.FromArgb(10, 16, 32, 64);
            m.Border = 1;
            m.Margin = 2;
            m.Padding = -2;
        }

        void ValidateMetaData(XElement e)
        {
            var md = e.Element(Namespace + "metadata");
            md.Should().HaveAttribute("background-color", "black");
            md.Should().HaveAttribute("border-color", "#0a102040");
            md.Attribute("text-color").Should().BeNull();

            md.Should().HaveAttribute("border", "1");
            md.Should().HaveAttribute("padding", "-2");
            md.Should().HaveAttribute("margin", "2");
        }

        void AssertMetaDataHandling<T>(T p, Func<T, XElement> writer)
            where T : IFormattingInfoProvider
        {
            SetUpMetaData(p.FormattingMetaData);
            var root = writer(p);
            ValidateMetaData(root);
        }

        [Test]
        public void ValidateTextureFileMetadataProperties()
        {
            AssertMetaDataHandling(new TextureSetFile(), TextureSetFileWriter.GenerateRoot);
        }

        [Test]
        public void ValidateEmptyTextureCollectionMetaData()
        {
            var tc = new TileTextureCollection();
            var root = TextureSetFileWriter.GenerateCollection(tc);

            root.Name.Should().Be(Namespace + "collection");
            root.Should().HaveAttribute("id", "");
            root.Should().HaveElement(Namespace + "metadata");
        }

        [Test]
        public void ValidateTextureCollectionMetadataProperties()
        {
            AssertMetaDataHandling(new TileTextureCollection(), TextureSetFileWriter.GenerateCollection);
        }

        [Test]
        public void ValidateEmptyTextureGridMetaData()
        {
            var tc = new TextureGrid();
            var root = TextureSetFileWriter.GenerateGrid(tc);

            root.Name.Should().Be(Namespace + "grid");
            root.Should().HaveAttribute("name", "");
            root.Should().HaveAttribute("x", "0");
            root.Should().HaveAttribute("y", "0");
            root.Attribute("cell-width").Should().BeNull();
            root.Attribute("cell-height").Should().BeNull();
            root.Should().HaveElement(Namespace + "metadata");
        }

        [Test]
        public void ValidateTextureGridOptionals()
        {
            var tc = new TextureGrid
            {
                AnchorX = 1,
                AnchorY = 2,
                Width = 3,
                Height = 4
            };
            var root = TextureSetFileWriter.GenerateGrid(tc);
            root.Should().HaveAttribute("anchor-x", "1");
            root.Should().HaveAttribute("anchor-y", "2");
            root.Element(Namespace + "metadata").Should().HaveAttribute("grid-width", "3");
            root.Element(Namespace + "metadata").Should().HaveAttribute("grid-height", "4");
        }

        [Test]
        public void ValidateTextureTile()
        {
            var tt = new TextureTile();
            var root = TextureSetFileWriter.GenerateTile(tt);

            root.Name.Should().Be(Namespace + "tile");
            root.Should().HaveAttribute("x", "0");
            root.Should().HaveAttribute("y", "0");
        }

        [Test]
        public void ValidateTextureTileSingleTag()
        {
            var tt = new TextureTile();
            tt.Tags.Add("tag1");
            var root = TextureSetFileWriter.GenerateTile(tt);

            root.Name.Should().Be(Namespace + "tile");
            root.Should().HaveAttribute("tag", "tag1");
            root.Should().HaveAttribute("x", "0");
            root.Should().HaveAttribute("y", "0");
        }

        [Test]
        public void ValidateTextureTileMultipleTags()
        {
            var tt = new TextureTile();
            tt.Tags.Add("tag1");
            tt.Tags.Add("tag2");
            var root = TextureSetFileWriter.GenerateTile(tt);

            root.Name.Should().Be(Namespace + "tile");
            root.Attribute("tag").Should().BeNull();
            root.Should().HaveAttribute("x", "0");
            root.Should().HaveAttribute("y", "0");

            var l = root.Elements(Namespace + "tag").ToList();
            l.Count.Should().Be(2);
            l[0].Should().HaveValue("tag1");
            l[1].Should().HaveValue("tag2");
        }

        [Test]
        public void ValidateTextureGridMetadataProperties()
        {
            AssertMetaDataHandling(new TextureGrid(), TextureSetFileWriter.GenerateGrid);
        }
    }
}
