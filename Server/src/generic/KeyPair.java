package generic;

public class KeyPair<K, V> {
	public K Key;
	public V Value;
	public KeyPair(K key, V value){
		Key = key;
		Value = value;
	}
}
