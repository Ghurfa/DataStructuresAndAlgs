#include <iostream>
#include "SinglyLinkedList.h"
using namespace std;

int main()
{
	int option = 0;
	SinglyLinkedList<int>* list = new SinglyLinkedList<int>();
	cout << "Options: " << endl <<
		"0. Add" << endl <<
		"1. AddAt" << endl <<
		"2. Remove Value" << endl <<
		"3. Remove At Index" << endl <<
		"4. Contains" << endl <<
		"5. Count" << endl <<
		"6. Exit" << endl;
	while (option != 6)
	{
		cin >> option;
		switch (option)
		{
		case 0:
		{
			int numToAdd;
			cout << "Number to add: ";
			cin >> numToAdd;
			list->Add(numToAdd);
			cout << "Added " << numToAdd << endl;
			break;
		}
		case 1:
		{
			int numToAdd;
			cout << "Number to add: ";
			cin >> numToAdd;
			int index = 0;
			cout << "Index to add at: ";
			cin >> index;
			list->AddAt(numToAdd, index);
			cout << "Added " << numToAdd << " at index " << index << endl;
			break;
		}
		case 2:
		{
			int numToRemove;
			cout << "Number to remove: ";
			cin >> numToRemove;
			bool result = list->Remove(numToRemove);
			if (result)
			{
				cout << "Removed value " << numToRemove << endl;
			}
			else
			{
				cout << "Failed to remove value " << numToRemove << endl;
			}
			break;
		}
		case 3:
		{
			int indexToRemoveAt;
			cout << "Index to remove at: ";
			cin >> indexToRemoveAt;
			bool result = list->RemoveAt(indexToRemoveAt);
			if (result)
			{
				cout << "Removed value at index " << indexToRemoveAt << endl;
			}
			else
			{
				cout << "Failed to remove value at index " << indexToRemoveAt << endl;
			}
			break;
		}
		case 4:
		{
			int numberToSearchFor = 0;
			cout << "Number to search for: ";
			cin >> numberToSearchFor;
			bool containsNum = list->Contains(numberToSearchFor);
			if (containsNum)
			{
				cout << "List contains " << numberToSearchFor << endl;
			}
			else
			{
				cout << "List does not contains " << numberToSearchFor << endl;
			}
			break;
		}
		case 5:
			cout << "Count: " << list->Count << endl;
			break;
		}
	}
	return 0;
}