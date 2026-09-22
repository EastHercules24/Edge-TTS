<h1>Edge TTS Server</h1>
<p>🌟 A Microsoft Edge TTS server coded in C# that listens to request and returns a file.</p>

<h2>Usage:</h2>
<h>When starting, it defaults to 127.0.0.1 and port 5000</h>
<p>Currently it supports: Http, (and that is currently everything there is :/)</p>
<p>When calling on the browser:</p>
<code>localhost:5000/?text=Your%20Text%20Here&voice=en-US-EmmaMultilingualNeural&filetype=mp3</code>
<p>You can also add:</p>
<code>&rate=+0%</code>
<code>&volume=+0%</code>
<code>&pitch=+0Hz</code>
<code>&boundarytype=SentenceBoundary</code>
<code>&proxy=...</code>

<h2>For Linux Users:</h2>
<h>Ok so... this is crazy to say but I found a way to run this program on linux with wine. Download this: </h>
<p>https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-10.0.5-windows-x64-installer</p>
<p>Then execute the program and it may work.</p>

<p>That's it for now again. :P</p>
