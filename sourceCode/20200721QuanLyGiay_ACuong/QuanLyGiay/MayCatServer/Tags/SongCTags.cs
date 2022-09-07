using MayCatServer.Tags;

namespace MayCatServer
{
    public class SongCTags : SongTags
    {
        public static SongCTags Instance { get; } = new SongCTags();

        public SongCTags() : base("SongC")
        {

        }
    }
}
