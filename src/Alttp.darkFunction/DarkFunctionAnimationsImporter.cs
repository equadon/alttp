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
    [ContentImporter(".anim", DisplayName = "darkFunction Animations Importer", DefaultProcessor = "DarkFunctionAnimationsProcessor")]
    public class DarkFunctionAnimationsImporter : ContentImporter<AnimationsData>
    {
        public override AnimationsData Import(string filename, ContentImporterContext context)
        {
            var doc = XDocument.Load(filename);
            var data = new AnimationsData(doc, filename);

            return data;
        }
    }
}
