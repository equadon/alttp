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
    [ContentProcessor(DisplayName = "darkFunction Animations Processor")]
    public class DarkFunctionAnimationsProcessor : ContentProcessor<AnimationsData, AnimationsData>
    {
        public override AnimationsData Process(AnimationsData input, ContentProcessorContext context)
        {
            // Save spritesheet texture
            var spriteSheetAsset = Path.Combine(input.FilePath, input.SpriteSheetFilename);

            string assetName;
            if (spriteSheetAsset.StartsWith(Directory.GetCurrentDirectory()))
                assetName = spriteSheetAsset.Substring(Directory.GetCurrentDirectory().Length + 1);
            else
                assetName = Path.GetFileName(spriteSheetAsset);

            // Remove file extension
            assetName = Path.Combine(Path.GetDirectoryName(assetName), Path.GetFileNameWithoutExtension(assetName));

            input.SpriteSheetRef = context.BuildAsset<SpriteSheetData, SpriteSheetData>(
                new ExternalReference<SpriteSheetData>(spriteSheetAsset),
                "DarkFunctionSpritesProcessor",
                null,
                "DarkFunctionSpritesImporter",
                assetName);

            return input;
        }
    }
}