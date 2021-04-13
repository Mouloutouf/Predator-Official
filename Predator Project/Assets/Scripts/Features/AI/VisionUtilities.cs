using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public static class VisionUtilities
    {
        public static Orientations ChangeOrientation(this EnemyManager enemy, int x, int y)
        {
            int eX, eY; enemy.GetEnemyPosition(out eX, out eY);

            if (x > eX)
            {
                if (y > eY) return Orientations.UpRight;
                else if (y < eY) return Orientations.DownRight;
                else return Orientations.Right;
            }
            else if (x < eX)
            {
                if (y > eY) return Orientations.UpLeft;
                else if (y < eY) return Orientations.DownLeft;
                else return Orientations.Left;
            }
            else
            {
                if (y > eY) return Orientations.Up;
                else if (y < eY) return Orientations.Down;

                else return enemy.orientation;
            }
        }

        public static void ChangeVisionConeAngle(this Image visionConeDisplay, Orientations orientation)
        {
            Vector3 newRotation = visionConeDisplay.transform.rotation.eulerAngles;

            switch (orientation)
            {
                case Orientations.Up: newRotation.z = 180; break;
                case Orientations.Right: newRotation.z = 90; break;
                case Orientations.Down: newRotation.z = 0; break;
                case Orientations.Left: newRotation.z = -90; break;
                case Orientations.UpRight: newRotation.z = 135; break;
                case Orientations.DownRight: newRotation.z = 45; break;
                case Orientations.DownLeft: newRotation.z = -45; break;
                case Orientations.UpLeft: newRotation.z = -135; break;
                default: break;
            }

            Quaternion quaternion = Quaternion.identity;
            quaternion.eulerAngles = newRotation;

            visionConeDisplay.transform.rotation = quaternion;
        }
    }
}
