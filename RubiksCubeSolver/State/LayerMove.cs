using System;

namespace VirtualRubik
{
    class LayerMove
    {

        public Cube3D.RubikPosition Layer;
        public Boolean Direction;

        public LayerMove(Cube3D.RubikPosition layer, Boolean direction)
        {
            Layer = layer;
            Direction = direction;
        }

    }
}
