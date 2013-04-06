using System;
using System.Collections.Generic;
using System.Linq;
using Alttp.DarkFunction.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Alttp.DarkFunction
{
    [ContentTypeWriter]
    public class DarkFunctionAnimationsWriter : ContentTypeWriter<AnimationsData>
    {
        protected override void Write(ContentWriter output, AnimationsData data)
        {
            output.WriteExternalReference(data.SpriteSheetRef);

            output.Write(data.Animations.Count);

            foreach (var animData in data.Animations)
            {
                output.Write(animData.Name);
                output.Write(animData.Loops);
                
                // frames
                output.Write(animData.Frames.Count);
                foreach (var frameData in animData.Frames)
                {
                    output.Write(frameData.Index);
                    output.Write(frameData.Delay);

                    // sprite refs
                    output.Write(frameData.Sprites.Count);
                    foreach (var spriteRef in frameData.Sprites)
                    {
                        output.Write(spriteRef.Name);
                        output.Write(spriteRef.X);
                        output.Write(spriteRef.Y);
                        output.Write(spriteRef.Z);
                        output.Write((decimal) spriteRef.Angle);
                        output.Write(spriteRef.FlipH);
                        output.Write(spriteRef.FlipV);
                    }
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Alttp.Core.Graphics.AnimationsReader, Alttp.Core";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "Alttp.Core.Graphics.AnimationsDict, Alttp.Core";
        }
    }
}
