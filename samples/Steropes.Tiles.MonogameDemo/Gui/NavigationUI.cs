using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Monogame;
using Steropes.Tiles.Navigation;
using Steropes.UI;
using Steropes.UI.Components;
using Steropes.UI.Input.KeyboardInput;
using Steropes.UI.Input.MouseInput;
using Steropes.UI.Styles;
using Steropes.UI.Util;
using Steropes.UI.Widgets;
using Steropes.UI.Widgets.Container;
using Steropes.UI.Widgets.TextWidgets;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace Steropes.Tiles.MonogameDemo.Gui
{
    /// <summary>
    ///   A basic UI to quickly navigate to map positions on the screen. Also handles the
    ///   dragging operation.
    /// </summary>
    public class NavigationUI : INotifyPropertyChanged
    {
        const double StepSize = .1;
        readonly NavigationModel navModel;
        readonly IUIManager uiManager;

        XnaPoint dragStartPosition;
        Group group;
        MapCoordinate tileUnderMouseCursor;
        ContinuousViewportCoordinates viewportPosition;

        public NavigationUI(IUIManager game, GameRendering gameRendering = null)
        {
            uiManager = game ?? throw new ArgumentNullException(nameof(game));
            GameRendering = gameRendering;
            navModel = new NavigationModel();

            RootComponent = SetupUi();
        }

        public IViewportControl GameRendering { get; set; }

        public IWidget RootComponent { get; }

        public string Title
        {
            set { uiManager.ScreenService.WindowService.Title = value; }
        }

        public MapCoordinate TileUnderMouseCursor
        {
            get { return tileUnderMouseCursor; }
            protected set
            {
                if (value.Equals(tileUnderMouseCursor))
                {
                    return;
                }

                tileUnderMouseCursor = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public static void SetupStyles(IUIManager manager)
        {
            var styleSystem = manager.UIStyle;
            styleSystem.StyleResolver.StyleRules.AddRange(styleSystem.LoadStyles("Content/UI/Metro/style.xml", "UI/Metro"));
            styleSystem.StyleResolver.StyleRules.AddRange(styleSystem.LoadStyles("Content/UI/extra-style.xml", "UI"));
        }

        IWidget SetupUi()
        {
            var styleSystem = uiManager.UIStyle;

            var mapX = new TextField(styleSystem);
            mapX.Anchor = AnchoredRect.CreateCentered(100);
            mapX.Content.Document.DocumentModified += (s, e) => navModel.MapXText = mapX.Text;

            var mapY = new TextField(styleSystem);
            mapY.Anchor = AnchoredRect.CreateCentered(100);
            mapY.Content.Document.DocumentModified += (s, e) => navModel.MapYText = mapY.Text;

            var button = new Button(styleSystem, "Go!");
            button.ActionPerformed += (sender, args) => navModel.TryNavigate(GameRendering);
            navModel.PropertyChanged += (s, e) => button.Enabled = navModel.Valid;

            var rotateLeftButton = new Button(styleSystem, "Left");
            rotateLeftButton.ActionPerformed +=
                (sender, args) => GameRendering.RotationSteps = (GameRendering.RotationSteps + 1) % 4;

            var rotateRightButton = new Button(styleSystem, "Right");
            rotateRightButton.ActionPerformed +=
                (sender, args) => GameRendering.RotationSteps = (GameRendering.RotationSteps - 1) % 4;

            var hbox = new BoxGroup(styleSystem, Orientation.Horizontal, 5);
            hbox.Anchor = AnchoredRect.CreateBottomLeftAnchored();
            hbox.AddStyleClass("opaque-root");
            hbox.Add(new Label(styleSystem, "Move to: X: "));
            hbox.Add(mapX);
            hbox.Add(new Label(styleSystem, "Y: "));
            hbox.Add(mapY);
            hbox.Add(button);
            hbox.Add(rotateLeftButton);
            hbox.Add(rotateRightButton);

            group = new Group(styleSystem);
            group.Add(hbox);
            group.Focusable = true;
            group.MouseDragged += OnMouseDragged;
            group.MouseDown += OnMouseDragStarted;
            group.MouseUp += OnMouseDragFinished;
            group.KeyReleased += Root_KeyReleased;
            group.MouseMoved += OnMouseMoved;
            group.Focused = true;
            group.Anchor = AnchoredRect.Full;
            return group;
        }

        void OnMouseMoved(object sender, MouseEventArgs e)
        {
            var pointedPosition = MouseRelativeToCenter(e.Position);
            var mouseInScreenScale =
                ContinuousViewportCoordinates.FromPixels(GameRendering.TileSize, pointedPosition.X, pointedPosition.Y);
            var posInScreenOrigin = GameRendering.CenterPoint + mouseInScreenScale;

            var mc = GameRendering.ScreenPositionToMapCoordinate(posInScreenOrigin.ToViewCoordinate());
            GameRendering.MapNavigator.NavigateTo(GridDirection.None, mc, out mc);
            TileUnderMouseCursor = mc;
        }

        DoublePoint MouseRelativeToCenter(XnaPoint p)
        {
            var ctr = group.LayoutRect.Center;
            return new DoublePoint(p.X - ctr.X, p.Y - ctr.Y);
        }

        void Root_KeyReleased(object sender, KeyEventArgs e)
        {
            var p = GameRendering.CenterPoint;

            switch (e.Key)
            {
                case Keys.NumPad4:
                case Keys.Left:
                {
                    p.X -= StepSize;
                    break;
                }
                case Keys.NumPad6:
                case Keys.Right:
                {
                    p.X += StepSize;
                    break;
                }
                case Keys.NumPad8:
                case Keys.Up:
                {
                    p.Y -= StepSize;
                    break;
                }
                case Keys.NumPad2:
                case Keys.Down:
                {
                    p.Y += StepSize;
                    break;
                }
                case Keys.Home:
                case Keys.NumPad7:
                {
                    p.X -= StepSize;
                    p.Y -= StepSize;
                    break;
                }
                case Keys.PageUp:
                case Keys.NumPad9:
                {
                    p.X += StepSize;
                    p.Y -= StepSize;
                    break;
                }
                case Keys.End:
                case Keys.NumPad1:
                {
                    p.X -= StepSize;
                    p.Y += StepSize;
                    break;
                }
                case Keys.PageDown:
                case Keys.NumPad3:
                {
                    p.X += StepSize;
                    p.Y += StepSize;
                    break;
                }
            }

            GameRendering.CenterPoint = p;
            e.Consume();
        }

        void OnMouseDragStarted(object sender, MouseEventArgs e)
        {
            dragStartPosition = e.Position;
            viewportPosition = GameRendering.CenterPoint;
        }

        void OnMouseDragged(object sender, MouseEventArgs e)
        {
            var delta = e.Position - dragStartPosition;
            var deltaInView = ContinuousViewportCoordinates.FromPixels(GameRendering.TileSize, delta.X, delta.Y);
            GameRendering.CenterPoint = viewportPosition - deltaInView;
        }

        void OnMouseDragFinished(object sender, MouseEventArgs e)
        {
            var delta = e.Position - dragStartPosition;
            var deltaInView = ContinuousViewportCoordinates.FromPixels(GameRendering.TileSize, delta.X, delta.Y);
            Debug.WriteLine("Finished move: " +
                            delta +
                            " - " +
                            (GameRendering.CenterPoint - deltaInView));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
