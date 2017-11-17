package generic;

import java.util.HashMap;

public class ReadOnlyHashMap<K, V> {
	private HashMap<K, V> hashMap;

	public ReadOnlyHashMap(HashMap nHashMap){
		hashMap = nHashMap;
	}

	public V get(K key){
		return hashMap.get(key);
	}

	public int size(){
		return hashMap.size();
	}

	public boolean isEmpty() {
		return hashMap.isEmpty();
	}

	public boolean containsKey(K key){
		return hashMap.containsKey(key);
	}

	public boolean containsValue(V value){
		return hashMap.containsValue(value);
	}

}
