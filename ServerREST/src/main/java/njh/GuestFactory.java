package njh;

import XMLechangeable.SessionInfo;

import java.util.HashSet;
import java.util.Random;

public class GuestFactory {
	private static Random random;
	private static HashSet<String> IdList;
	private final static IdGenerator idGenerator = new IdGenerator();
	static {
		random = new Random(Thread.currentThread().getId() * Runtime.getRuntime().freeMemory());
		IdList = new HashSet<>();
	}


	public static GuestImpl GetNewGuest(RegisterImpl register){
		return new GuestImpl(register, "guest" + idGenerator.GetId(),GetId());
	}

	public static void DisposeGuest(GuestImpl guest){
		IdList.remove(guest.Password);
		idGenerator.ReturnId(guest.getUsername().replace("guest", ""));
	}

	private static String GetId(){
		String newID;
		while (IdList.contains((newID = BuildId())));
		return newID;
	}

	private static String BuildId(){
		String newID = Int2AlphaNumeric.Convert(random.nextLong());
		newID += Int2AlphaNumeric.Convert(random.nextLong());
		return newID;
	}
}
