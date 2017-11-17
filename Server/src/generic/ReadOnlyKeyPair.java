package generic;

public class ReadOnlyKeyPair<K,V> {
	private KeyPair<K,V> keyPair;

	public ReadOnlyKeyPair(KeyPair nKeyPair){
		keyPair = nKeyPair;
	}

	public K getKey(){
		return keyPair.Key;
	}

	public V getValue(){
		return keyPair.Value;
	}

}
