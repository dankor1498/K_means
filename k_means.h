#pragma once
#ifndef K_MEANS_H_
#define K_MEANS_H_

#include <iostream>
#include <cstdlib>
#include <ctime>
#include <fstream> 
#include <sstream>
#include <vector>
#include <cmath>
#include <Windows.h>

const int N = 50; //���������� �������� ��� ��������� ��������� ������
const int KK = 10; //���������� ���������
const int max_iterations = 100; //������������ ���������� ��������

typedef struct {            //�������
	double r;
	double g;
	double b;
} rgb;

void form_an_image(std::ostream & st); //������� ������ � ���� 
                                       //������� �������

class K_means
{
private:
	std::vector<rgb> pixcel; //������ ��������
	int q_klaster;           //���������� ���������
	int k_pixcel;            //���������� ��������
	std::vector<rgb> centr;  //������ �������������
	void identify_centers(); //����� ���������� ������ ��������� �������
	inline double compute(rgb k1, rgb k2)
	{
		return sqrt(pow((k1.r - k2.r),2) + pow((k1.g - k2.g), 2) + pow((k1.b - k2.b), 2));
	}
	inline double compute_s(double a, double b) {
		return (a + b) / 2;
	};
public:
	K_means() : q_klaster(0), k_pixcel(0) {}; //������������
	K_means(int n, rgb *mas, int n_klaster);
	K_means(int n_klaster, std::istream & os);
	void clustering(std::ostream & os); //����� �������������
	void print()const; //����� ������
	~K_means();
	friend std::ostream & operator<<(std::ostream & os, const K_means & k); //������������ �������� << 
	};

#endif // !K_MEANS_H_



