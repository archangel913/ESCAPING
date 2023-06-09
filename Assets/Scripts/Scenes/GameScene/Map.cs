namespace Escaping.GameScene
{
    /// <summary>
    /// マップの地形情報
    /// </summary>
    public enum MapStructure
    {
        /// <summary>
        /// 壁
        /// </summary>
        Wall = 0,

        /// <summary>
        /// 道
        /// </summary>
        Path = 1,
    }

    /// <summary>
    /// マップクラス
    /// </summary>
    public class Map
    {
        private MapStructure[,] structure;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="structure">マップデータ</param>
        /// <param name="mapSize">マップサイズ</param>
        public Map(MapStructure[,] structure, int mapSize)
        {
            this.structure = structure;
            MapSize = mapSize;
        }

        /// <summary>
        /// マップの一辺のサイズ
        /// </summary>
        public int MapSize { get; }

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
        /// <exception cref="System.ArgumentOutOfRangeException">与えられた引数がマップ外の時</exception>
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
}
