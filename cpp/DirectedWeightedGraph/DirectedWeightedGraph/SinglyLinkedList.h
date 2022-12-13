#pragma once
#include "SinglyLinkedNode.h"
template <typename T>
class SinglyLinkedList
{
public:
	const int& Count = count;
	SinglyLinkedList();
	~SinglyLinkedList();
	void Add(T value);
	void AddAt(T value, int index); //AddBefore(node, value) AddAfter AddFirst AddLast
	bool Remove(T value);
	bool RemoveAt(int index);
	bool Contains(T value);
	void CopyTo(T* arr, int index = 0);
private:
	int count;
	SinglyLinkedNode<T>* head;
};

template <typename T>
SinglyLinkedList<T>::SinglyLinkedList()
{
	head = nullptr;
	count = 0;
}

template <typename T>
SinglyLinkedList<T>::~SinglyLinkedList()
{
	//store a pointer to the next node
	//delete the current node
	//repeat
	SinglyLinkedNode<T>* current = head;
	SinglyLinkedNode<T>* next;
	while (current != nullptr)
	{
		next = current->Next;
		delete(current);
		current = next;
	}
}

template <typename T>
void SinglyLinkedList<T>::Add(T value)
{
	if (head == nullptr)
	{
		head = new SinglyLinkedNode<T>(value);
	}
	else
	{
		SinglyLinkedNode<T>* traverser;
		for (traverser = head; traverser->Next != nullptr; traverser = traverser->Next) {}
		traverser->Next = new SinglyLinkedNode<T>(value);
	}
	count++;
}

template <typename T>
void SinglyLinkedList<T>::AddAt(T value, int index)
{
	if (index < 0 || index >= count) 
		throw std::out_of_range("Index is out of range");
	if (index == 0)
	{
		head = new SinglyLinkedNode<T>(value, head);
	}
	else
	{
		SinglyLinkedNode<T>* traverser = head;
		for (int i = 0; i < index - 1; i++)
		{
			traverser = traverser->Next;
		}
		traverser->Next = new SinglyLinkedNode<T>(value, traverser->Next);
	}
	count++;
}

template <typename T>
bool SinglyLinkedList<T>::Remove(T value)
{
	SinglyLinkedNode<T>* traverser;
	if (head->Value == value)
	{
		SinglyLinkedNode<T>* newHead = head->Next;
		delete(head);
		head = newHead;
		return true;
	}
	for (traverser = head; traverser->Next != nullptr; traverser = traverser->Next)
	{
		if (traverser->Next->Value == value)
		{
			SinglyLinkedNode<T>* newNext = traverser->Next->Next;
			delete(traverser->Next);
			traverser->Next = newNext;
			return true;
		}
	}
	return false;
}

template <typename T>
bool SinglyLinkedList<T>::RemoveAt(int index)
{
	if (index < 0 || index >= count)
	{
		return false;
	}
	if (index == 0)
	{
		SinglyLinkedNode<T>* newHead = head->Next;
		delete(head);
		head = newHead;
		return true;
	}
	SinglyLinkedNode<T>* traverser = head;
	for (int i = 0; i < index - 1; i++)
	{
		traverser = traverser->Next;
	}
	SinglyLinkedNode<T>* newNext = traverser->Next->Next;
	delete(traverser->Next);
	traverser->Next = newNext;
	return true;
}

template <typename T>
bool SinglyLinkedList<T>::Contains(T value)
{
	SinglyLinkedNode<T>* traverser;
	for (traverser = head; traverser != nullptr; traverser = traverser->Next)
	{
		if (traverser->Value == value)
		{
			return true;
		}
	}
	return false;
}

template <typename T>
void SinglyLinkedList<T>::CopyTo(T* arr, int startIndex)
{
	T* index = arr + startIndex * sizeof(T);
	SinglyLinkedNode<T>* node = head;
	for (int i = 0; i < count; i++)
	{
		*index = node->Value;
		index += sizeof(T);
		node = node->Next;
	}
}