namespace _Game.Scripts.Gameplay.Utils
{
    using System.Collections.Generic;
    using BlockSmash.Entities;
    using UnityEngine;

    public static class GridUtils
    {
        public static void AlignBlocksToCells(Transform parent, List<Block> blocks)
        {
            if (blocks == null || blocks.Count == 0)
                return;

            var firstBlock = blocks[0];
            if (firstBlock.CurrentCell == null)
            {
                Debug.LogError("First block has no CurrentCell assigned.");
                return;
            }

            var offset = firstBlock.CurrentCell.transform.position - firstBlock.transform.position;

            parent.position += offset;
        }
    }
}