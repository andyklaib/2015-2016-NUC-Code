#pragma once


#include<iostream>
#include<stdlib.h>
#include<climits>
#include<cerrno>
#include<opencv2/opencv.hpp>
#include<opencv2/imgproc/imgproc.hpp>
#include<opencv2/highgui/highgui.hpp>
#include<opencv2/video/tracking.hpp>
#include<math.h>
#include <string.h>

//#include<


using namespace std;
using namespace cv;


enum STR2INT_ERROR {SUCCESS = 0,OVERFLOWE,UNDERFLOWE, INCONVERTIBLE};

struct Target{
	int CenterX;
	int CenterY;
	int tlx;
	int tly;
	int brx;
	int bry;
	float angle;
	int Radius;
	bool valid;
	int FramesSinceLast;
};
struct ErrorMessage{
	int Number;
	char* Exception;
	ErrorMessage(int n){Number = n;};
	ErrorMessage(int n,char* s){Number = n; Exception = s;};
};

namespace LeahDLL{
	public ref class Leah
	{
		
	public:
		Leah(){
		VIDEOSOURCE = 2;
		ImgWidth = 1280;
		ImgHeight = 720;
		DEBUG = 1;
		THRESHOLD = 50;
		CurrentTarget = new Target();
		};
		virtual void run();
		virtual void run(int VideoSource,int ImgWidth,int ImgHeight,int Threshold,bool Debug);
		virtual void calDetect(uchar* buffer,int size);
		virtual Target* GetTarget();
		virtual void process(uchar* buffer,int size);
	private:
		//STR2INT_ERROR str2int(int &i,char const *s,int base = 0);
		//System::Drawing::Bitmap^ Leah::MatToBitmap(const cv::Mat& img)
		static Target* CurrentTarget;
		static VideoCapture* capture;
		static Mat* Vidframe;
		static int VIDEOSOURCE ;
		static int ImgWidth;
		static int ImgHeight;
		static bool DEBUG ;
		static int THRESHOLD ;
		
	};
}