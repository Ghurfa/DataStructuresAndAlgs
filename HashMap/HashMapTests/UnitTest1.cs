using HashMap;

namespace HashMapTests
{
    public class UnitTest1
    {
        [Fact]
        public void ResetTest()
        {
            HashMap<int, string> map = new HashMap<int, string>();
            map.Add(2, "2");
            map.Clear();
            Assert.Empty(map);
            Assert.False(map.ContainsKey(2));
        }

        [Theory]
        [InlineData(2, "2")]
        public void AddTest(int key, string value)
        {
            HashMap<int, string> map = new HashMap<int, string>();
            map.Add(key, value);
            Assert.Single(map);
            Assert.True(map.ContainsKey(key));
            Assert.Contains(key, map.Keys);
            Assert.Contains(value, map.Values);
            Assert.Equal(value, map[key]);
        }
    }
}