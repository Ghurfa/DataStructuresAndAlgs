#pragma once;
#include <iostream>;
#include <math.h>;
using namespace std;

template <typename T>
class MinHeap
{
public:
	MinHeap();
	const int& Count = count;
	void Insert(T value);
	T Pop();
	void Clear();
	void Display();
private:
	int* arr;
	int count;
	int capacity;
	inline int parent(int index);
	inline int leftChild(int index);
	inline int rightChild(int index);
	void heapifyUp(int index);
	void heapifyDown();
	void swapValues(int firstIndex, int secondIndex);
	void checkCapacity();
	void changeSize(int newSize);
};

template <typename T>
MinHeap<T>::MinHeap()
{
	count = 0;
	capacity = 8;
	arr = new T[capacity];
}

template <typename T>
void MinHeap<T>::Insert(T value)
{
	arr[count] = value;
	heapifyUp(count);
	count++;
	checkCapacity();
}

template <typename T>
T MinHeap<T>::Pop()
{
	if (count == 0) throw new out_of_range("Heap is Empty");
	T retVal = arr[0];
	arr[0] = arr[count - 1];
	count--;
	heapifyDown();
	checkCapacity();
	return retVal;
}

template <typename T>
void MinHeap<T>::Clear()
{
	delete[](arr);
	count = 0;
	capacity = 8;
	arr = new T[capacity];
}

template <typename T>
void MinHeap<T>::Display()
{
	for (int i = 0; i < count; i++)
	{
		float level = log2f(i + 1);
		if (level == (int)level)
		{
			cout << endl;
		}
		cout << arr[i] << " ";
	}
	cout << endl;
}

template <typename T>
inline int MinHeap<T>::parent(int index)
{
	return (ceil((float)index / 2)) - 1;
}

template <typename T>
inline int MinHeap<T>::leftChild(int index)
{
	return index * 2 + 1;
}

template <typename T>
inline int MinHeap<T>::rightChild(int index)
{
	return index * 2 + 2;
}

template <typename T>
void MinHeap<T>::heapifyUp(int index)
{
	int parentIndex = parent(index);
	while (index > 0 && arr[parentIndex] > arr[index])
	{
		swapValues(index, parentIndex);
		index = parentIndex;
		parentIndex = parent(index);
	}
}

template <typename T>
void MinHeap<T>::heapifyDown()
{
	int index = 0;
	int leftChildIndex = leftChild(index);
	int rightChildIndex = rightChild(index);
	while (leftChildIndex < count)
	{
		if (arr[leftChildIndex] < arr[index])
		{
			swapValues(index, leftChildIndex);
			index = leftChildIndex;
		}
		else if (rightChildIndex >= count || arr[rightChildIndex] >= arr[index])
		{
			break;
		}
		else
		{
			swapValues(index, rightChildIndex);
			index = rightChildIndex;
		}
		leftChildIndex = leftChild(index);
		rightChildIndex = rightChild(index);
	}
}

template <typename T>
void MinHeap<T>::swapValues(int firstIndex, int secondIndex)
{
	int temp = arr[secondIndex];
	arr[secondIndex] = arr[firstIndex];
	arr[firstIndex] = temp;
}

template <typename T>
void MinHeap<T>::checkCapacity()
{
	if (count == capacity)
	{
		changeSize(capacity * 2);
	}
	else if (count < capacity / 2)
	{
		changeSize(capacity / 2);
	}
}

template <typename T>
void MinHeap<T>::changeSize(int newSize)
{
	int* temp = arr;
	capacity  = newSize;
	arr = new int[capacity];
	for (int i = 0; i < newSize; i++)
	{
		arr[i] = temp[i];
	}
}