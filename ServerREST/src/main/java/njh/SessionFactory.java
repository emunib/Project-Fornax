package njh;

import XMLechangeable.SessionInfo;

import java.util.HashSet;
import java.util.Random;

public class SessionFactory {
	private static Random random;
	private static HashSet<String> IdList;
	static {
		random = new Random(Thread.currentThread().getId() * Runtime.getRuntime().freeMemory());
		IdList = new HashSet<>();
	}


	public static SessionImpl GetNewSession(PlayerImpl player){
		return new SessionImpl(player, GetId(), GetId());
	}

	public static void DiposeSession(SessionImpl session){
		IdList.remove(session.getPrivateID());
		IdList.remove(session.getPublicID());
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
