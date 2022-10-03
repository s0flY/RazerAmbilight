using System.Drawing;
using Ambilight.GUI;
using Colore;
using Colore.Effects.ChromaLink;
using ColoreColor = Colore.Data.Color;


namespace Ambilight.Logic
{

    /// <summary>
    /// Handles the Ambilight Effect for the Link connection
    /// </summary>
    internal class LinkLogic : IDeviceLogic
    {
        private readonly TraySettings _settings;
        private readonly IChroma _chroma;
        private CustomChromaLinkEffect _linkGrid = CustomChromaLinkEffect.Create();
        
        public LinkLogic(TraySettings settings, IChroma chromaInstance)
        {
            _settings = settings;
            _chroma = chromaInstance;
        }

        /// <summary>
        /// Processes a ScreenShot and creates an Ambilight Effect for the chroma link
        /// </summary>
        /// <param name="newImage">ScreenShot</param>
        public void Process(Bitmap newImage)
        {
            var map = ImageManipulation.ResizeImage(newImage, 8, 5);
            map = ImageManipulation.ApplySaturation(map, _settings.Saturation);
            
            ApplyImageToGrid(map);
            ApplyC1(ImageManipulation.ResizeImage(map, 1, 1));
            _chroma.ChromaLink.SetCustomAsync(_linkGrid);
            map.Dispose();
        }

        private void ApplyC1(Bitmap map)
        {
            var color = map.GetPixel(0,0);
            _linkGrid[0] = new ColoreColor(color.R, color.G, color.B);
        }

        /// <summary>
        /// From a given resized screenshot, an ambilight effect will be created for the keyboard
        /// Specified link configuration only for two devices in group2 and group3
        /// </summary>
        /// <param name="map">resized screenshot</param>
        private void ApplyImageToGrid(Bitmap map)
        {
            var rightBulbColor = map.GetPixel(7, 2);
            _linkGrid[1] = new ColoreColor(rightBulbColor.R, rightBulbColor.G, rightBulbColor.B);
            var leftBulbColor = map.GetPixel(0,2);
            _linkGrid[2] = new ColoreColor(leftBulbColor.R, leftBulbColor.G, leftBulbColor.B);
        }
    }
}
