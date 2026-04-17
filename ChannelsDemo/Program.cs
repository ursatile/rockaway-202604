// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Threading.Channels;

var s = new Stopwatch();

Console.WriteLine("Hello, World!");

const int COUNT = 10;
var c = Channel.CreateUnbounded<int>();

s.Start();
for (var i = 0; i < COUNT; i++) await c.Writer.WriteAsync(i);
Console.WriteLine($"Wrote {COUNT} things into the channel - it took {s.ElapsedMilliseconds}ms");

while (true) {
	var nextWorkItem = await c.Reader.ReadAsync();
	Console.WriteLine(nextWorkItem);
	Thread.Sleep(TimeSpan.FromSeconds(1));
}