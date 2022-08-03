using MayCatServer.Tags;

namespace MayCatServer
{
    public class SongBTags : SongTags
    {
        public static SongBTags Instance { get; } = new SongBTags();

        public SongBTags() : base("SongB")
        {
        }
    }
}
