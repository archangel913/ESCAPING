namespace Escaping.GameScene
{
    /// <summary>
    /// マップクラス
    /// </summary>
    public class Map
    {
        private MapStructure[,] structure;

        public int MapSize { get; }

        public Map(MapStructure[,] structure, int mapSize)
        {
            this.structure = structure;
            MapSize = mapSize;
        }

        /// <summary>
        /// 壁と道の位置情報を拾得する
        /// </summary>
        /// <returns>MapStructureの2次元配列</returns>
        public MapStructure[,] GetMap()
        {
            return this.structure;
        }

        /// <summary>
        /// ある地点[x,y]の地形情報を拾得する
        /// </summary>
        /// <param name="x">拾得したいx座標</param>
        /// <param name="y">拾得したいy座標</param>
        /// <returns>地形情報</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public MapStructure GetStructureAt(int x, int y)
        {
            if (MapSize > x && MapSize > y && x >= 0 && y >= 0)
            {
                return this.structure[x, y];
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("x or y is out of range");
            }
        }
    }

    /// <summary>
    /// マップの地形情報
    /// </summary>
    public enum MapStructure
    {
        Wall = 0,
        Path = 1
    }
}
