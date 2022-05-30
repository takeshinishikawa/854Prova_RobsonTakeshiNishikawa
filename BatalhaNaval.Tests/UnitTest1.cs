using BatalhaNaval;

namespace BatalhaNaval.Tests
{
    public class UnitTest1
    {
        [Fact]
        public static void Test1()
        {
            string answer = "";
            bool isMultiplayer = false;
            singleOrMulti(ref answer, ref isMultiplayer);
        }
    }
}