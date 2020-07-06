#include <iostream>
#include "List.h"
using namespace std;

int main()
{
	int option = 0;
	List<int>* list = new List<int>();
	cout << "Options: " << endl << "0. Add" << endl << "1. Remove" << endl << "2. Contains" << endl << "3. Exit" << endl;
	while (option != 3)
	{
		cin >> option;
		switch (option)
		{
		case 0:
		{
			int numToAdd = 0;
			cout << "Number to add: ";
			cin >> numToAdd;
			list->Add(numToAdd);
			cout << "Added " << numToAdd << endl;
			break;
		}
		case 1:
		{
			int index = 0;
			cout << "Index to delete at: ";
			cin >> index;
			if (list->Remove(index))
			{
				cout << "Deleted at index " << index << endl;
			}
			else
			{
				cout << "Failed to delete at index " << index << endl;
			}
			break;
		}
		case 2:
		{
			int numToLookFor = 0;
			cout << "Number to look for: ";
			cin >> numToLookFor;
			if (list->Contains(numToLookFor))
			{
				cout << "List contains " << numToLookFor << endl;
			}
			else
			{
				cout << "List does not contain " << numToLookFor << endl;
			}
			break;
		}
		}
	}
	return 0;
}