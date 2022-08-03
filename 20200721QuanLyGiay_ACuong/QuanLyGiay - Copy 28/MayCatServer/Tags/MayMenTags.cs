using MayCatServer.Tags;

namespace MayCatServer
{
    public class MayMenTags : SongTags
    {
        public static MayMenTags Instance { get; } = new MayMenTags();

        public MayMenTags() : base("MayMen")
        {
        }
    }
}
