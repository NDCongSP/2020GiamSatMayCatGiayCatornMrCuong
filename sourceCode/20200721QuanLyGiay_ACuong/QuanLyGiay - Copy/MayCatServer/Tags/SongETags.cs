using MayCatServer.Tags;

namespace MayCatServer
{
    public class SongETags : SongTags
    {
        public static SongETags Instance { get; } = new SongETags();

        public SongETags() : base("SongE")
        {
        }
    }
}
