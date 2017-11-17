package generic;

public class KeyPair<K, V> implements Cloneable {
	public K Key;
	public V Value;
	public KeyPair(K key, V value){
		Key = key;
		Value = value;
	}

	public Object clone(){
		return new KeyPair<>(Key, Value);
	}
}
