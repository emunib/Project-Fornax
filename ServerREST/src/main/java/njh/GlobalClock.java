package njh;

import java.util.Timer;
import java.util.TimerTask;

public class GlobalClock {
	static long ticks;
	static Timer timer;
	static TimerTask timerTask = new TimerTask() {
		@Override
		public void run() {
			ticks++;
		}
	};
	static {
		timer = new Timer();
		timer.schedule(timerTask, 0, 1000);
	}

	public static long GetTime(){
		return ticks;
	}
}
