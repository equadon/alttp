using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Alttp.DarkFunction.Data
{
    public class AnimationsData
    {
        public string Filename { get; set; }
        public string FilePath { get; set; }

        public ExternalReference<SpriteSheetData> SpriteSheetRef { get; set; }

        public string SpriteSheetFilename { get; set; }

        public List<AnimationData> Animations { get; set; }

        public AnimationsData(XDocument doc, string filename)
        {
            Filename = Path.GetFileName(filename);
            FilePath = Path.GetDirectoryName(filename);

            var root = doc.Element("animations");

            SpriteSheetFilename = (string)root.Attribute("spriteSheet");

            LoadAnimations(root.Elements("anim"));
        }

        public void LoadAnimations(IEnumerable<XElement> animations)
        {
            Animations = new List<AnimationData>();

            foreach (var animation in animations)
            {
                var frames = LoadFrames(animation.Elements("cell"));

                var anim = new AnimationData()
                    {
                        Name = (string)animation.Attribute("name"),
                        Loops = (int)animation.Attribute("loops"),
                        Frames = frames
                    };

                Animations.Add(anim);
            }
        }

        private List<FrameData> LoadFrames(IEnumerable<XElement> cells)
        {
            var frames = new List<FrameData>();

            foreach (var cell in cells)
            {
                var sprites = LoadSprites(cell.Elements("spr"));

                var frame = new FrameData()
                    {
                        Index = (int)cell.Attribute("index"),
                        Delay = (int)cell.Attribute("delay"),
                        Sprites = sprites
                    };

                frames.Add(frame);
            }

            return frames;
        }

        private List<SpriteRefData> LoadSprites(IEnumerable<XElement> spriteElements)
        {
            var sprites = new List<SpriteRefData>();

            foreach (var spr in spriteElements)
            {
                var sprite = new SpriteRefData()
                    {
                        Name = (string)spr.Attribute("name"),
                        X = (int)spr.Attribute("x"),
                        Y = (int)spr.Attribute("y"),
                        Z = (int)spr.Attribute("z"),
                        Angle = 0f,
                        FlipH = false,
                        FlipV = false
                    };

                var elem = spr.Attribute("angle");
                if (elem != null)
                    sprite.Angle = (float) elem;

                elem = spr.Attribute("flipH");
                if (elem != null)
                    sprite.FlipH = (bool)elem;

                elem = spr.Attribute("flipV");
                if (elem != null)
                    sprite.FlipV = (bool)elem;

                sprites.Add(sprite);
            }

            return sprites;
        }
    }
}
