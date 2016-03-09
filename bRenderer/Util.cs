using Microsoft.Xna.Framework;
using System;

/** @brief Static utility functions.
*	@author Benjamin Buergisser
*/
class Util
{
    public static float getMaxVectorValue(Vector3 vector)
    {
        float max = vector.X;
        if (vector.Y > max) max = vector.Y;
        if (vector.Z > max) max = vector.Z;
        return max;
    }

    public static float getMaxAbsVectorValue(Vector3 vector)
    {
        float max = Math.Abs(vector.X);
        if (Math.Abs(vector.Y) > max) max = Math.Abs(vector.Y);
        if (Math.Abs(vector.Z) > max) max = Math.Abs(vector.Z);
        return max;
    }
}