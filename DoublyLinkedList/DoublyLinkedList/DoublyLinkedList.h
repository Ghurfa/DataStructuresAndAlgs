#pragma once
#include <memory>
#include "DoublyLinkedNode.h"
using namespace std;

template <typename T>
class DoublyLinkedList
{
public:
	DoublyLinkedList();
	const int& Count = count;
	void AddFirst(T value);
	void AddLast(T value);
	void AddBefore(T value, DoublyLinkedNode<T>* node);
	void AddAfter(T value, DoublyLinkedNode<T>* node);
	bool RemoveFirst();
	bool RemoveLast();
	bool RemoveBefore(DoublyLinkedNode<T>*);
	bool RemoveAfter(DoublyLinkedNode<T>*);
	bool Contains(T value);
	void Clear();
private:
	DoublyLinkedNode<T>* head;
	//shared_ptr<DoublyLinkedNode<T>> tail;
	int count;
};

template <typename T>
DoublyLinkedList<T>::DoublyLinkedList()
{
	head = nullptr;
	count = 0;
}


template <typename T>
void DoublyLinkedList<T>::AddFirst(T value)
{
	auto newNode = make_unique<DoublyLinkedNode<T>>(value);
	if (count == 0)
	{
		head = newNode.get();
		newNode->Previous = newNode.get();
		newNode->Next = move(newNode);
	}
	else
	{
		newNode->Next = move(head->Previous->Next);
		DoublyLinkedNode<T>* newNodePtr = newNode.get();
		head->Previous->Next = move(newNode);
		newNodePtr->Previous = head->Previous;
		head->Previous = newNodePtr;

		head = newNodePtr;
	}
	count++;
}

template <typename T>
void DoublyLinkedList<T>::AddLast(T value)
{
	auto newNode = make_unique<DoublyLinkedNode<T>>(value);
	if (count == 0)
	{
		head = newNode.get();
		newNode->Previous = newNode.get();
		newNode->Next = move(newNode);
	}
	else
	{
		newNode->Next = move(head->Previous->Next);
		DoublyLinkedNode<T>* newNodePtr = newNode.get();
		head->Previous->Next = move(newNode);
		newNodePtr->Previous = head->Previous;
		head->Previous = newNodePtr;
	}
	count++;
}
template <typename T>
void DoublyLinkedList<T>::AddBefore(T value, DoublyLinkedNode<T>* node)
{
	auto newNode = make_unique<DoublyLinkedNode<T>>(value);
	node->Previous = newNode.get();
	newNode.get()->Next = move(node->Previous->Next);
	newNode.get()->Previous = node->Previous;
	newNode.get()->Previous.Next = move(newNode);
}
template <typename T>
void DoublyLinkedList<T>::AddAfter(T value, DoublyLinkedNode<T>* node)
{
	auto newNode = make_unique<DoublyLinkedNode<T>>(value);
	node->Next.get()->Previous = newNode.get();
	newNode.get()->Next = move(node->Next);
	newNode.get()->Previous = node;
	node->Next = move(newNode);
}
template <typename T>
bool DoublyLinkedList<T>::RemoveFirst()
{
	if (count == 0) return false;
	else if (count == 1)
	{
		head->Next = nullptr;
		head = nullptr;
	}
	else
	{
		DoublyLinkedNode<T>* newHead = head->Next.get();
		DoublyLinkedNode<T>* tail = head->Previous;
		tail->Next = move(head->Next);
		newHead->Previous = tail;
		head = newHead;
	}
	count--;
	return true;
}
template <typename T>
bool DoublyLinkedList<T>::RemoveLast()
{
	if (count == 0) return false;
	else if (count == 1)
	{
		head->Next = nullptr;
		head = nullptr;
	}
	else
	{
		DoublyLinkedNode<T>* newTail = head->Previous->Previous;
		newTail->Next = move(head->Previous->Next);
		head->Previous = newTail;
	}
	count--;
	return true;
}
template <typename T>
bool DoublyLinkedList<T>::RemoveBefore(DoublyLinkedNode<T>* node)
{
	if (node->Previous == head) return RemoveFirst();

	DoublyLinkedNode<T>* newPrevious = node->Previous->Previous;
	newPrevious->Next = move(node->Previous->Next());
	node->Previous = newPrevious;
}
template <typename T>
bool DoublyLinkedList<T>::RemoveAfter(DoublyLinkedNode<T>* node)
{
	if (node->Next.get() == head) return RemoveFirst();

	DoublyLinkedNode<T>* newNext = node->Next.get()->Next.get();
	node->Next = newNext;
	newNext->Previous = node;
}
template <typename T>
bool DoublyLinkedList<T>::Contains(T value)
{
	DoublyLinkedNode<T>* traverser = head;
	for (int i = 0; i < count; i++)
	{
		if (traverser->Value == value) return true;
		traverser = traverser->Previous;
	}
	return false;
}
template <typename T>
void DoublyLinkedList<T>::Clear()
{
	if (count == 0) return;
	unique_ptr<DoublyLinkedNode<T>> weapon;// = make_unique<DoublyLinkedNode<T>>(0);
	weapon = move(head->Previous->Next);
	while (weapon.get()->Next != nullptr)
	{
		weapon = move(weapon.get()->Next);
	}
	head = nullptr;
	count = 0;
}