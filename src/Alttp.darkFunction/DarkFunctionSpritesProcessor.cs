using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Alttp.DarkFunction.Data;
using Alttp.DarkFunction.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace Alttp.DarkFunction
{
    [ContentProcessor(DisplayName = "darkFunction Sprites Processor")]
    public class DarkFunctionSpritesProcessor : ContentProcessor<SpriteSheetData, SpriteSheetData>
    {
        public override SpriteSheetData Process(SpriteSheetData input, ContentProcessorContext context)
        {
            // Save spritesheet texture
            var sourceAsset = Path.Combine(input.FilePath, input.ImageFilename);

            string assetName;
            if (sourceAsset.StartsWith(Directory.GetCurrentDirectory()))
                assetName = sourceAsset.Substring(Directory.GetCurrentDirectory().Length + 1);
            else
                assetName = Path.GetFileName(sourceAsset);

            // Remove file extension
            assetName = Path.Combine(Path.GetDirectoryName(assetName), Path.GetFileNameWithoutExtension(assetName));

            input.ImageRef = context.BuildAsset<TextureContent, TextureContent>(
                new ExternalReference<TextureContent>(sourceAsset),
                "TextureProcessor",
                null,
                "TextureImporter",
                assetName);

            return input;
        }
    }
}