using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Alttp.DarkFunction.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Alttp.DarkFunction
{
    [ContentImporter(".sprites", DisplayName = "darkFunction Sprites Importer", DefaultProcessor = "DarkFunctionSpritesProcessor")]
    public class DarkFunctionSpritesImporter : ContentImporter<SpriteSheetData>
    {
        public override SpriteSheetData Import(string filename, ContentImporterContext context)
        {
            var doc = XDocument.Load(filename);
            var data = new SpriteSheetData(doc, filename);

            return data;
        }
    }
}
