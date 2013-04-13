using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Alttp.DarkFunction.Data
{
    public class SpriteSheetData
    {
        public string Filename { get; set; }
        public string FilePath { get; set; }

        public ExternalReference<TextureContent> ImageRef { get; set; }

        public string ImageFilename { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        public Dictionary<string, List<SpriteData>> Sprites { get; set; }

        public SpriteSheetData(XDocument doc, string filename)
        {
            Filename = Path.GetFileName(filename);
            FilePath = Path.GetDirectoryName(filename);

            Sprites = new Dictionary<string, List<SpriteData>>();

            var root = doc.Element("img");

            ImageFilename = (string)root.Attribute("name");
            ImageWidth = (int)root.Attribute("w");
            ImageHeight = (int)root.Attribute("h");

            LoadSprites(root.Element("definitions"));
        }

        public void LoadSprites(XElement definitions)
        {
            var sprsp = definitions.Descendants("spr");
            // Loop through sprites
            foreach (var xSpr in definitions.Descendants("spr"))
            {
                var dirs = GetDirs(xSpr, new List<string>());
                dirs.Reverse();

                string key = "/" + string.Join("/", dirs);

                if (!Sprites.ContainsKey(key))
                    Sprites[key] = new List<SpriteData>();

                var sprite = new SpriteData()
                    {
                        Name = (string)xSpr.Attribute("name"),
                        Path = key,
                        X = (int)xSpr.Attribute("x"),
                        Y = (int)xSpr.Attribute("y"),
                        Width = (int)xSpr.Attribute("w"),
                        Height = (int)xSpr.Attribute("h"),
                    };
                Sprites[key].Add(sprite);
            }
        }

        private List<string> GetDirs(XElement e, List<string> dirs)
        {
            if (e.Parent.Name == "dir" && e.Parent.Attribute("name").Value != "/")
            {
                dirs.Add((string) e.Parent.Attribute("name"));
                dirs = GetDirs(e.Parent, dirs);
            }

            return dirs;
        }
    }
}
