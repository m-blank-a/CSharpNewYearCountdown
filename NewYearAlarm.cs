using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Timer = System.Windows.Forms.Timer;

namespace CSharpNewYearCountdown
{
    internal class NewYearAlarm
    {
        // January 1, 2026 00:00:00 (GMT+08:00)
        // change ts for next year
        const int UNIXNEWYEAR = 1767196800;

        // Offset time to play song earlier than alarm
        // Chorus of Lauv - I Like Me Better starts at 49.8s
        const int secondsOffset = 49;
        const int millisecondsOffset = 800;

        // Note: When using a sound, set sampling rate to 96kHz
        //       and set to Unsigned 32-bit PCM. In this way, the
        //       SoundPlayer knows that it is a lengthy song.
        //       It often goes with InvalidOp or AccessViolation errors
        //       if both sampling rate and PCM codec do not match or is
        //       not large enough to match the audio's length.
        static Stream audioStream = Properties.Resources.NewYearTheme;
        const short CHECKINTERVAL = 20;
        static DateTimeOffset localTimestamp =
            DateTimeOffset.FromUnixTimeSeconds(UNIXNEWYEAR);
        DateTime alarmTime = localTimestamp.LocalDateTime;

        // Moved to class-level scope since this might get GC'ed
        MemoryStream memory = new MemoryStream();
        SoundPlayer player;
        bool canPlay = false;

        internal Timer NewYearTimer { get; set; }

        internal NewYearAlarm()
        {
            alarmTime = alarmTime.AddSeconds(-secondsOffset)
                .AddMilliseconds(-millisecondsOffset);

            NewYearTimer = new Timer();
            NewYearTimer.Interval = CHECKINTERVAL;
            // Add time update event handler function
            NewYearTimer.Tick += new EventHandler(CheckTime);

            // Put audio data to memory buffer
            audioStream.CopyTo(memory);
            memory.Position = 0;

            // Create a SoundPlayer object with the audio data in 'memory'
            // and preload to avoid errors.
            player = new SoundPlayer(memory);
            player.LoadCompleted += (s, e) => canPlay = true;
            player.LoadAsync();

            // Enable timer
            NewYearTimer.Enabled = true;
        }

        private void CheckTime(object sender, EventArgs e) {
            DateTime currentTime = DateTime.Now;
            if (currentTime.Hour == alarmTime.Hour
                && currentTime.Minute >= alarmTime.Minute
                && currentTime.Second >= alarmTime.Second
                && canPlay)
            {
                player.PlayLooping();
                NewYearTimer.Stop();
            }
        }

        internal void Abort() {
            NewYearTimer.Stop();
            NewYearTimer.Dispose();
        }
    }
}
