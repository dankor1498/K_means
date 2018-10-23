#include "pch.h"
#include "k_means.h"

void form_an_image(std::ostream & st) {
	srand((unsigned)time(NULL));
	for (int i = 1; i <= N * 3; i++) {
		st << 0 + rand() % 256 << " ";
		if (i % 3 == 0) st << std::endl;
	}
}

void K_means::identify_centers()
{
	srand((unsigned)time(NULL));
	rgb temp;
	rgb *mas = new rgb[q_klaster];
	for (int i = 0; i < q_klaster; i++) {
		temp = pixcel[0 + rand() % k_pixcel];
		for (int j = i; j < q_klaster; j++) {
			if (temp.r != mas[j].r && temp.g != mas[j].g && temp.b != mas[j].b) {
				mas[j] = temp;
			}
			else {
				i--;
				break;
			}
		}
	}
	for (int i = 0; i < q_klaster; i++) {
		centr.push_back(mas[i]);
	}
	delete []mas;
}

K_means::K_means(int n, rgb * mas, int n_klaster)
{
	for (int i = 0; i < n; i++) {
		pixcel.push_back(*(mas + i));
	}
	q_klaster = n_klaster;
	k_pixcel = n;
	identify_centers();
}

K_means::K_means(int n_klaster, std::istream & os) : q_klaster(n_klaster)
{
	rgb temp;
	while (os >> temp.r && os >> temp.g && os >> temp.b) {
		pixcel.push_back(temp);
	}
	k_pixcel = pixcel.size();
	identify_centers();
}

void K_means::clustering(std::ostream & os)
{
	os << "\n\nНачало кластеризации:" << std::endl;

	std::vector<int> check_1(k_pixcel, -1);
	std::vector<int> check_2(k_pixcel, -2);
	int iter = 0;

	while(true)
	{
		os << "\n\n---------------- Итерация №" 
			<< iter << " ----------------\n\n";
		{
			for (int j = 0; j < k_pixcel; j++) {
				double *mas = new double[q_klaster];

				for (int i = 0; i < q_klaster; i++) {
					*(mas + i) = compute(pixcel[j], centr[i]);
					os << "Расстояние от пикселя " << j << " к центру #" 
						<< i << ": " << *(mas + i) << std::endl;
				}

				double min_dist = *mas;
				int m_k = 0;
				for (int i = 0; i < q_klaster; i++) {
					if (min_dist > *(mas + i)) {
						min_dist = *(mas + i);
						m_k = i;
					}
				}
				os << "Минимальное расстояние к центру #"  << m_k << std::endl;
				os << "Пересчитываем центр #" << m_k << ": ";
				centr[m_k].r = compute_s(pixcel[j].r, centr[m_k].r);
				centr[m_k].g = compute_s(pixcel[j].g, centr[m_k].g);
				centr[m_k].b = compute_s(pixcel[j].b, centr[m_k].b);
				os << centr[m_k].r << " " << centr[m_k].g 
					<< " " << centr[m_k].b << std::endl;
				delete[] mas;
			}

			int *mass = new int[k_pixcel];
			os << "\nПроведем классификацию пикселей: "<< std::endl;
			for (int k = 0; k < k_pixcel; k++) {
				double *mas = new double[q_klaster];

				for (int i = 0; i < q_klaster; i++) {
					*(mas + i) = compute(pixcel[k], centr[i]);
					os << "Расстояние от пикселя №" << k << " к центру #" 
						<< i << ": " << *(mas + i) << std::endl;
				}

				double min_dist = *mas;
				int m_k = 0;
				for (int i = 0; i < q_klaster; i++) {
					if (min_dist > *(mas + i)) {
						min_dist = *(mas + i);
						m_k = i;
					}
				}
				mass[k] = m_k;
				os << "Пиксель №" << k << " ближе всего к центру #" << m_k << std::endl;
			}

			os << "\nМассив соответствия пикселей и центров: \n";
			for (int i = 0; i < k_pixcel; i++) {
				os << mass[i] << " ";
				check_1[i] = *(mass + i);
			}
			os << std::endl << std::endl;

			os << "Результат кластеризации: " << std::endl;
			int itr = KK + 1;
			for (int i = 0; i < q_klaster; i++) {
				os << "Кластер #" << i << std::endl;
				for (int j = 0; j < k_pixcel; j++) {
					if (mass[j] == i) {
						os << pixcel[j].r << " " << pixcel[j].g
							<< " " << pixcel[j].b << std::endl;
						mass[j] = ++itr;
					}
				}
			}

			delete[] mass;

			os << "Новые центры: \n" ;
			for (int i = 0; i < q_klaster; i++) {
				os << centr[i].r << " " << centr[i].g
					<< " " << centr[i].b << " - #" << i << std::endl;
			}
		}
		iter++;
		if (check_1 == check_2 || iter >= max_iterations) {
			break;
		}
		check_2 = check_1;
	}
	os << "\n\nКонец кластеризации." << std::endl;
}

void K_means::print() const
{
	std::cout << "Начальные пиксели: " << std::endl;
	for (int i = 0; i < k_pixcel; i++) {
		std::cout << pixcel[i].r << " " << pixcel[i].g
			<< " " << pixcel[i].b << " - №" << i << std::endl;
	}
	std::cout << std::endl << "Случайные начальные центры кластеризации: " << std::endl;
	for (int i = 0; i < q_klaster; i++) {
		std::cout << centr[i].r << " " << centr[i].g << " "
			<< centr[i].b << " - #" << i << std::endl;
	}
	std::cout << "\nКоличество кластеров: " << q_klaster << std::endl;
	std::cout << "Количество пикселей: " << k_pixcel << std::endl;
}

std::ostream & operator<<(std::ostream & os, const K_means & k)
{
	os << "Начальные пиксели: " << std::endl;
	for (int i = 0; i < k.k_pixcel; i++) {
		os << k.pixcel[i].r << " " << k.pixcel[i].g
			<< " " << k.pixcel[i].b << " - №" << i << std::endl;
	}
	os << std::endl << "Случайные начальные центры кластеризации: " << std::endl;
	for (int i = 0; i < k.q_klaster; i++) {
		os << k.centr[i].r << " " << k.centr[i].g << " "
			<< k.centr[i].b << " - #" << i << std::endl;
	}
	os << "\nКоличество кластеров: " << k.q_klaster << std::endl;
	os << "Количество пикселей: " << k.k_pixcel << std::endl;
	return os;
}

K_means::~K_means()
{
}
