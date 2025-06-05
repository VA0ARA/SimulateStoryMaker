using System;
using System.Windows;
using System.Windows.Media;
using StoryMaker.Helpers;

namespace StoryMaker.Models.Utility
{
    public static class TransformUtility
    {
        public static Vector2 CalculatePosition(this Matrix matrix)
        {
            return new Vector2((float)matrix.OffsetX, (float)matrix.OffsetY);
        }

        public static float CalculateAngle(this Matrix matrix)
        {
            return (float)(Math.Atan2(-matrix.M21, matrix.M22) * Mathf.RadToDeg);
        }

        public static Vector2 CalculateScale(this Matrix matrix)
        {
            
            var scaleX = Math.Sign(matrix.M11) * Math.Sqrt(Math.Pow(matrix.M11, 2) + Math.Pow(matrix.M12, 2));
            var scaleY = Math.Sign(matrix.M22) * Math.Sqrt(Math.Pow(matrix.M21, 2) + Math.Pow(matrix.M22, 2));

             return new Vector2((float)scaleX, (float)scaleY);
        }

   /*     public static void CalculateTransformation(this Matrix matrix, out Vector2 position, out float angle,
            out Vector2 scale)
        {
            position=matrix.CalculatePosition();
            angle =  matrix.CalculateAngle();
            scale =  matrix.CalculateScale();
        }*/

        public static Matrix GetWorldMatrix(this Matrix localMatrix, Matrix coordinateMatrix)
        {
            localMatrix.Append(coordinateMatrix);
            return localMatrix;
        }

        public static Matrix GetLocalMatrix(this Matrix worldMatrix, Matrix coordinateMatrix)
        {
            coordinateMatrix.Invert();
            worldMatrix.Append(coordinateMatrix);
            return worldMatrix;
        }

        // public static Vector2 CalculateWorldPosition(Transform parent, Vector2 localPosition)
        // {
        //     if (parent == null)
        //         return localPosition;
        //
        //     var matrix = new Matrix(1, 0, 0, 1, localPosition.X, localPosition.Y);
        //
        //     if (Math.Abs(parent.Angle) > 0)
        //         matrix.Rotate(parent.Angle);
        //
        //     if (parent.Scale != Vector2.One)
        //         matrix.Scale(parent.Scale.X, parent.Scale.Y);
        //
        //     matrix.OffsetX += parent.Position.X;
        //     matrix.OffsetY += parent.Position.Y;
        //
        //     var worldPos = new Vector2((float) matrix.OffsetX, (float) matrix.OffsetY);
        //
        //     return worldPos;
        // }
        //
        // public static Vector2 CalculateLocalPosition(Transform parent, Vector2 worldPosition)
        // {
        //     if (parent == null)
        //         return worldPosition;
        //
        //     var matrix = new Matrix(1, 0, 0, 1, worldPosition.X - parent.Position.X,
        //         worldPosition.Y - parent.Position.Y);
        //
        //     if (parent.Scale != Vector2.One)
        //         matrix.Scale(1 / parent.Scale.X, 1 / parent.Scale.Y);
        //
        //     if (Math.Abs(parent.Angle) > 0)
        //         matrix.Rotate(-parent.Angle);
        //
        //     var localPosition = new Vector2((float) matrix.OffsetX, (float) matrix.OffsetY);
        //
        //     return localPosition;
        // }
        //
        //
        // public static Vector2 CalculateWorldScale(Transform parent, Vector2 localScale, float localAngle)
        // {
        //     if (parent == null)
        //         return localScale;
        //
        //     var child = Matrix.Identity;
        //     child.Rotate(localAngle);
        //     child.Scale(localScale.X, localScale.Y);
        //
        //     var parentMatrix = Matrix.Identity;
        //     parentMatrix.Scale(parent.LocalScale.X, parent.LocalScale.Y);
        //
        //     child.Append(parentMatrix);
        //     child.CalculateScale(out var scale);
        //     return scale;
        //
        //     // var quartersCount = (float) Math.Floor(localAngle / 90);
        //     // var quartersAngle = quartersCount * 90;
        //     // var quarterOneOrThree = quartersCount % 2 == 0;
        //     //
        //     // var absAngle = quarterOneOrThree ? 90 - (localAngle - quartersAngle) : localAngle - quartersAngle;
        //     //
        //     // var xAngle = absAngle;
        //     // var yAngle = 90 - absAngle;
        //     //
        //     // var angleNormalize = new Vector2(xAngle / 90, yAngle / 90);
        //     //
        //     // var xScale = parent.Scale.X * angleNormalize.X + parent.Scale.Y * angleNormalize.Y;
        //     // var yScale = parent.Scale.Y * angleNormalize.X + parent.Scale.X * angleNormalize.Y;
        //
        //     // return new Vector2(xScale * localScale.X, yScale * localScale.Y);
        // }
        //
        // public static Vector2 CalculateLocalScale(Transform parent, Vector2 worldScale, float worldAngle)
        // {
        //     if (parent == null)
        //         return worldScale;
        //
        //     var localAngle = Math.Abs(CalculateLocalAngle(parent, worldAngle) * 2);
        //
        //     var sin = (float) Math.Sin(localAngle);
        //     var x = Math.Abs(sin) > 0 ? worldScale.X / parent.Scale.X / sin : 0;
        //
        //     var cos = (float) Math.Cos(localAngle);
        //     var y = Math.Abs(cos) > 0 ? worldScale.Y / parent.Scale.Y / cos : 0;
        //
        //     return new Vector2(x, y);
        // }
        //
        // public static float CalculateWorldAngle(Transform parent, float localAngle)
        // {
        //     if (parent == null)
        //         return localAngle;
        //
        //     var worldAngle = parent.Angle + localAngle;
        //     return worldAngle;
        // }
        //
        // public static float CalculateLocalAngle(Transform parent, float worldAngle)
        // {
        //     if (parent == null)
        //         return worldAngle;
        //
        //     var localAngle = worldAngle - parent.Angle;
        //     return localAngle;
        // }
    }
}