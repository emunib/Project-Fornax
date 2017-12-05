package Common;

import java.util.ArrayList;

public class IdGenerator {
	private final ArrayList<String> Pool = new ArrayList<>();
	private long count;

	public IdGenerator(){
		count = 0;
	}

	public synchronized String GetId(){
		String result;
		if (Pool.isEmpty()){
			count++;
			result = Int2AlphaNumeric.Convert(count);
		} else {
			result = Pool.remove(0);
		}
		return result;
	}

	public synchronized void ReturnId(String id){
		Pool.add(id);
	}

}
