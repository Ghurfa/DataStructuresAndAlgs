#pragma once
template <typename T>
class List
{
public:
	const int& Capacity = arrCapacity;
	const int& Count = count;
	List(int = 7);
	void Add(T);
	bool Remove(T);
	bool RemoveLast();
	bool Contains(T);
private:
	T* valuesArr;
	int arrCapacity;
	int count;
	void expand();
};

//include tpp here

template <typename T>
List<T>::List(int capacity)
{
	arrCapacity = capacity;
	valuesArr = new T[capacity];
}

template <typename T>
void List<T>::Add(T value)
{
	if (count == arrCapacity) expand();
	valuesArr[count] = value;
	count++;
}

template <typename T>
bool List<T>::Remove(T index)
{
	if (index < 0 || count <= index) return false;
	for (int i = index; i < count - 1; i++)
	{
		valuesArr[i] = valuesArr[i + 1];
	}
	count--;
	return true;
}

template <typename T>
bool List<T>::RemoveLast()
{
	if (count < 1) return false;
	count--;
	return true;
}

template <typename T>
bool List<T>::Contains(T value)
{
	for (int i = 0; i < count; i++)
	{
		if (valuesArr[i] == value) return true;
	}
	return false;
}

template <typename T>
void List<T>::expand()
{
	T* temp = new T[arrCapacity * 2];
	for (int i = 0; i < count; i++)
	{
		temp[i] = valuesArr[i];
	}
	delete[] valuesArr;
	valuesArr = temp;
}