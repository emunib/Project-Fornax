package njh;

public class Counter {
	int Value;
	public Counter(){
		Value = 0;
	}

	public int IncUp(){
		int temp = Value;
		Value += 1;
		return temp;
	}
}
