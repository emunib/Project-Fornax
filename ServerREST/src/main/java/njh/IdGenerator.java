package njh;

import java.util.ArrayList;

class Int2AlphaNumeric {
	private final static Character[] table = new Character[]{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
	};
	public static String Convert(long value){
		StringBuilder builder = new StringBuilder();
		while (value > 0){
			int temp = (int) value % 62;
			value = value / 62;
			builder.append(table[temp]);
		}
		return builder.reverse().toString();
	}
}

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
