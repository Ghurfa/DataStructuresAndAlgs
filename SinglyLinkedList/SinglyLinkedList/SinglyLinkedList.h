#pragma once
#include "SinglyLinkedNode.h"

template <typename T>
class SinglyLinkedList
{
public:
	struct iterator
	{
	public:
		using difference_type = T;
		using value_type = T;
		using pointer = const T*;
		using reference = const T&;
		using iterator_category = std::forward_iterator_tag;

		iterator(SinglyLinkedNode<T>* ptr) { this->ptr = ptr; }
		reference operator*() const { return ptr->Value; }
		iterator& operator++() { ptr = ptr->Next.get(); return *this; }
		iterator operator++(T)
		{
			iterator temp = *this;
			++(*this);
			return temp;
		}
		bool operator== (iterator other) const { return ptr == other.ptr; };
		bool operator!= (iterator other) const { return ptr != other.ptr; };

	private:
		SinglyLinkedNode<T>* ptr;
	};
	const int& Count = count;
	SinglyLinkedList();
	~SinglyLinkedList();
	void AddFirst(T value);
	void AddLast(T value);
	void AddAt(T value, int index);
	void Clear();
	bool Contains(T value);
	void CopyTo(T* arr, int index = 0);
	bool Remove(T value);
	bool RemoveAt(int index);
	iterator begin();
	iterator end();
private:
	int count;
	std::unique_ptr<SinglyLinkedNode<T>> head;
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

}

template <typename T>
void SinglyLinkedList<T>::AddFirst(T value)
{
	head = std::make_unique<SinglyLinkedNode<T>>(value, std::move(head));
	count++;
}

template <typename T>
void SinglyLinkedList<T>::AddLast(T value)
{
	if (head == nullptr)
	{
		head = std::make_unique<SinglyLinkedNode<T>>(value);
	}
	else
	{
		SinglyLinkedNode<T>* traverser;
		for (traverser = head.get(); traverser->Next != nullptr; traverser = traverser->Next.get())
		{

		}
		traverser->Next = std::make_unique<SinglyLinkedNode<T>>(value);
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
		head = std::make_unique<SinglyLinkedNode<T>>(value, std::move(head));
	}
	else
	{
		SinglyLinkedNode<T>* traverser = head.get();
		for (int i = 0; i < index - 1; i++)
		{
			traverser = traverser->Next.get();
		}
		traverser->Next = std::make_unique<SinglyLinkedNode<T>>(value, std::move(traverser->Next));
	}
	count++;
}

template <typename T>
void SinglyLinkedList<T>::Clear()
{
	//Works, but stackoverflows
	// 
	//auto temp = std::move(head);
	//

	while (head)
	{
		head = std::move(head->Next);
	}
	count = 0;
}

template <typename T>
bool SinglyLinkedList<T>::Contains(T value)
{
	SinglyLinkedNode<T>* traverser;
	for (traverser = head.get(); traverser != nullptr; traverser = traverser->Next.get())
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
	T* index = arr + startIndex;
	SinglyLinkedNode<T>* node = head.get();
	for (int i = 0; i < count; i++)
	{
		*index = node->Value;
		index++;
		node = node->Next.get();
	}
}

template <typename T>
bool SinglyLinkedList<T>::Remove(T value)
{
	SinglyLinkedNode<T>* traverser;
	if (head->Value == value)
	{
		head = std::move(head->Next);
		count--;
		return true;
	}
	for (traverser = head.get(); traverser->Next != nullptr; traverser = traverser->Next.get())
	{
		if (traverser->Next->Value == value)
		{
			traverser->Next = std::move(traverser->Next->Next);
			count--;
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
		head = std::move(head->Next);
		count--;
		return true;
	}
	SinglyLinkedNode<T>* traverser = head.get();
	for (int i = 0; i < index - 1; i++)
	{
		traverser = traverser->Next.get();
	}
	traverser->Next = std::move(traverser->Next->Next);
	count--;
	return true;
}

template<typename T>
typename SinglyLinkedList<T>::iterator SinglyLinkedList<T>::begin()
{
	return iterator(head.get());
}

template<typename T>
typename SinglyLinkedList<T>::iterator SinglyLinkedList<T>::end()
{
	return iterator(nullptr);
}