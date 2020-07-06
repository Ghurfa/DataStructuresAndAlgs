#pragma once
template <typename T>
class SinglyLinkedNode
{
public:
	T Value;
	SinglyLinkedNode<T>* Next;
	SinglyLinkedNode(T value, SinglyLinkedNode<T>* next = nullptr);
};

template<typename T>
SinglyLinkedNode<T>::SinglyLinkedNode(T value, SinglyLinkedNode<T>* next)
{
	Value = value;
	Next = next;
}
