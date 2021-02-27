using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class VisionArea
    {
        public Dictionary<Orientations, List<Vector2Int>> areaVisions = new Dictionary<Orientations, List<Vector2Int>>();

        public (int b, int a)[] straightAreaVisionPositions = new (int b, int a)[]
        {
            (0, 1), (0, 2), (0, 3), (0, 4), (0, 5),
            (1, 1), (1, 2), (1, 3), (1, 4), (1, 5),
            (-1, 1), (-1, 2), (-1, 3), (-1, 4), (-1, 5),
            (2, 3), (2, 4), (2, 5),
            (-2, 3), (-2, 4), (-2, 5)
        };

        public (int x, int y)[] diagonalAreaVisionPositions = new (int x, int y)[]
        {
            (1, 0), (2, 0),     (0, 1), (0, 2),
            (1, 1), (2, 2), (3, 3),
            (2, 1), (3, 1), (4, 1),     (1, 2), (1, 3), (1, 4),
            (3, 2), (4, 2), (5, 2), (4, 3),     (2, 3), (2, 4), (2, 5), (3, 4)
        };

        public VisionArea()
        {
            AddStraightAreas(Orientations.Right);
            AddStraightAreas(Orientations.Left);
            AddStraightAreas(Orientations.Up);
            AddStraightAreas(Orientations.Down);

            AddDiagonalAreas(Orientations.UpRight);
            AddDiagonalAreas(Orientations.UpLeft);
            AddDiagonalAreas(Orientations.DownLeft);
            AddDiagonalAreas(Orientations.DownRight);
        }

        private void AddStraightAreas(Orientations orientation)
        {
            int x = 0;
            int y = 0;
            List<Vector2Int> positions = new List<Vector2Int>();
            foreach ((int b, int a) tuple in straightAreaVisionPositions)
            {
                switch (orientation)
                {
                    case Orientations.Up:  x = tuple.b; y = tuple.a; break;
                    case Orientations.Right: x = tuple.a; y = tuple.b; break;
                    case Orientations.Down: x = tuple.b; y = -tuple.a; break;
                    case Orientations.Left: x = -tuple.a; y = tuple.b; break;
                }

                positions.Add(new Vector2Int(x, y));
            }
            areaVisions.Add(orientation, positions);
        }

        private void AddDiagonalAreas(Orientations orientation)
        {
            int x = 0;
            int y = 0;
            List<Vector2Int> positions = new List<Vector2Int>();
            foreach ((int x, int y) tuple in diagonalAreaVisionPositions)
            {
                switch (orientation)
                {
                    case Orientations.UpRight: x = tuple.x; y = tuple.y; break;
                    case Orientations.UpLeft: x = -tuple.x; y = tuple.y; break;
                    case Orientations.DownLeft: x = -tuple.x; y = -tuple.y; break;
                    case Orientations.DownRight: x = tuple.x; y = -tuple.y; break;
                }

                positions.Add(new Vector2Int(x, y));
            }
            areaVisions.Add(orientation, positions);
        }
    }
}