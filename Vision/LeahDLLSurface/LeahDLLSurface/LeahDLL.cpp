// LeahDLL.cpp : Defines the exported functions for the DLL application.
//
//#include "stdafx.h"
#include"LeahDLL.h"

using namespace LeahDLL;

#define PIE 3.1457
#define MaxRange 5000 //30000
#define INITALVALUE 50
#define SCALE 0.1

int hist(VideoCapture);

void Leah::run()
{
	VideoCapture capture(VIDEOSOURCE);//Make connection to camera
    capture.set(CV_CAP_PROP_FPS,30);//Set frame rate
	capture.set(CV_CAP_PROP_FRAME_WIDTH,ImgWidth);//Set image width
	capture.set(CV_CAP_PROP_FRAME_HEIGHT,ImgHeight);//Set image Height
	if(!capture.isOpened()){//test if camera connection was successful
            cout<< "Failed to connect to the camera." << endl;
            return ;
    }
    else{
            cout << "Camera connected successfully." << endl;
    }
	//hist(capture);//Execute main code
	//Initalize Target
	CurrentTarget->valid = false;
	CurrentTarget->FramesSinceLast = 0;
	return;
}
void Leah::run(int VideoSource,int IW,int IH,int Threshold,bool Debug,uchar* mapPtr,int size)
{
	VIDEOSOURCE = VideoSource;
	ImgWidth = IW;
	ImgHeight = IH;
	THRESHOLD = Threshold;
	DEBUG = Debug;
	VideoCapture* Lcapture = new VideoCapture(VIDEOSOURCE);//Make connection to camera
    Lcapture->set(CV_CAP_PROP_FPS,30);//Set frame rate
	Lcapture->set(CV_CAP_PROP_FRAME_WIDTH,ImgWidth);//Set image width
	Lcapture->set(CV_CAP_PROP_FRAME_HEIGHT,ImgHeight);//Set image Height
	if(!Lcapture->isOpened()){//test if camera connection was successful
            //cout<< "Failed to connect to the camera." << endl;
			throw ErrorMessage(0,"Failed to connect Camera");
    }
    else{
            //cout << "Camera connected successfully." << endl;
    }
	MapSize = size;
	CollisionMap = new Mat();
	CollisionMap->rows = MapSize;
	CollisionMap->cols = MapSize;
	//CollisionMap->
	CollisionMap->data = mapPtr;
	//Initalize Target
	CurrentTarget = new Target();
	CurrentTarget->valid = false;
	CurrentTarget->FramesSinceLast = 0;
	capture = Lcapture;
	//hist(capture);//Execute main code

	return ;
}

void Leah::process(uchar* buffer,int size)//Main processign function
{
	Mat frame,mask;//Camera frame container
	Mat referance = imread("referance.png");//Load histogram referance image
#pragma region Historgram Creation
	cvtColor(referance,referance,CV_BGR2HSV);
	MatND hist;
	int h_bins = 30; int s_bins = 32;
	int histSize[] = { h_bins, s_bins };
	float h_range[] = { 0, 179 };
	float s_range[] = { 0, 255 };
	const float* ranges[] = { h_range, s_range };
	int channels[] = { 0, 1 };
	calcHist( &referance, 1, channels, Mat(), hist, 2, histSize, ranges, true, false );
	normalize( hist, hist, 0, 255, NORM_MINMAX, -1, Mat() );
#pragma endregion

	vector<vector<Point> > contours,contoursTwo;
	vector<Vec4i> hierarchy;
	int64 start,stop;
	double time;
	//while(1)//Processing loop
	//{
		start = getTickCount();
		Mat target,passed;
		*capture >> frame;//Capturea frame from the cmaera and place it in frame
		pyrDown(frame,mask,Size(frame.cols / 2,frame.rows /2));//Scale down the sourcei mage by a factor of 4
		pyrDown(mask,mask,Size(mask.cols / 2,mask.rows /2));//This is done to reduce noise and increase framerate

		cvtColor(mask,mask,CV_BGR2HSV);//Color space conversion for more accurate color detection

		calcBackProject( &mask, 1, channels, hist, passed, ranges, 2, true );
		blur(passed,passed,Size(5,5));
		threshold(passed,passed,THRESHOLD ,255,THRESH_BINARY);
		Canny(passed,passed,30,90);
		int dilation_size = 5;
		Mat element = getStructuringElement( MORPH_RECT,
                                Size( 2*dilation_size + 1, 2*dilation_size+1 ),
                                Point( dilation_size, dilation_size ) );
		dilate(passed,passed,element);
		findContours( passed, contours, hierarchy, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_SIMPLE, Point(0, 0) );
		vector<vector<Point> > contours_poly( contours.size() );
		vector<Rect> boxes( contours.size() );
		for( int j = 0; j < contours.size(); j++ )
		{ 
			approxPolyDP( Mat(contours[j]), contours_poly[j], 3, true );
			boxes[j] = boundingRect( Mat(contours_poly[j]) );

			rectangle(frame,boxes[j].tl() * 4,boxes[j].br() * 4,Scalar(0,0,255),2,8,0);

		}
		//Incrament the frame counter. If the target is found the count will be zeroed anyways
		CurrentTarget->FramesSinceLast++;

		//Test if we can do a second pass
		for(int i = 0; i < boxes.size();i++)
		{
			if(boxes[i].area() > 625)
			{
				//cout << "Entering second pass big box: " << i << '\n';
				Mat passTwo,maskTwo,sourceTwo;
				vector<Vec3f> circles;
				vector<Vec4i> hierarchyTwo;
				Rect test = Rect(boxes[i].tl() * 4,boxes[i].br() *4);
				frame(test).copyTo(sourceTwo);
				
				cvtColor(sourceTwo,maskTwo,CV_BGR2HSV);//Color space conversion for more accurate color detection
				blur(maskTwo,maskTwo,Size(5,5));
				calcBackProject( &maskTwo, 1, channels, hist, passTwo, ranges, 2, true );
				threshold(passTwo,passTwo,THRESHOLD ,255,THRESH_BINARY);
				dilate(passTwo,passTwo,element);
				//imshow("passTwo",passTwo);
				findContours( passTwo, contoursTwo, hierarchyTwo, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_SIMPLE, Point(0, 0) );
				vector<vector<Point> > contours_poly( contoursTwo.size() );
				vector<Rect> boxesTwo( contoursTwo.size() );
				vector<Point2f>center( contoursTwo.size() );
				vector<float>radius( contoursTwo.size() );
				vector<double>area (contoursTwo.size() );
				//cout << "Number of shapes: "<< contoursTwo.size() <<'\n';
				for( int j = 0; j < contoursTwo.size(); j++ )
				{ 
					//cout << "Pass " << j << '\n';
					approxPolyDP( Mat(contoursTwo[j]), contours_poly[j], 3, true );
					boxesTwo[j] = boundingRect( Mat(contours_poly[j]) );
					area[j] = contourArea(contoursTwo[j]);
					rectangle(sourceTwo,boxesTwo[j].tl() ,boxesTwo[j].br() ,Scalar(0,255,0),2,8,0);
					minEnclosingCircle( (Mat)contours_poly[j], center[j], radius[j] );
					
				}
				for( int j = 0; j < center.size(); j++ )
				{
					//cout << "Circle test \n"; 
					float cArea = (3.14 * radius[j] * radius[j]);
					if((area[j] * 0.785 > cArea - cArea*0.40 ) )//&& (area[j] * 0.785 < cArea + cArea*0.10 ))//0.40 /*boxesTwo[j].area()*/
					{
						//cout << "Circle found \n";
						circle( sourceTwo, Point(center[j]) , (int)radius[j] , Scalar(255,0,0), 2, 8, 0 );
						putText(frame,"Target",Point(center[j])  + boxes[i].tl() * 4,0,1,Scalar(255,0,0),2,8,false);
						CurrentTarget->FramesSinceLast = 0;
						CurrentTarget->valid = true;
						CurrentTarget->CenterX = center[j].x + boxes[i].tl().x;
						CurrentTarget->CenterY = center[j].y + boxes[i].tl().y;
						CurrentTarget->Radius = radius[j];

					}
				}
				//imshow("maskTwo",sourceTwo);
			}
		}
		
//Loop delay block

		stop = getTickCount();
		time = (stop - start)/getTickFrequency();
		int chan = frame.channels();
		int col = frame.cols * chan;
		//putText(frame,std::to_string(time),Point(100,100),0,1,Scalar(255,255,255),2,8,false);
		putText(frame,std::to_string(1/time),Point(100,200),0,1,Scalar(255,255,255),2,8,false);
		//putText(frame,std::to_string(frame.rows),Point(100,50),0,1,Scalar(255,0,0),2,8,false);
		//putText(frame,std::to_string(frame.cols),Point(100,100),0,1,Scalar(0,255,0),2,8,false);
		//putText(frame,std::to_string(chan),Point(100,150),0,1,Scalar(0,0,255),2,8,false);
		//putText(frame,std::to_string(col),Point(200,50),0,1,Scalar(0,0,255),2,8,false);
		imshow("Input",frame);
		//imshow("passed",passed);
		waitKey(5);
	//}

		//Vidframe = new Mat(frame.rows,frame.cols,CV_8UC3);
		//imwrite("test.jpg",frame);
		//frame.copyTo(*Vidframe);

		//int i,j;
		//
		//uchar* p;
		//if(size >= frame.rows * col)
		//{
		//	for(i = 0; i < frame.rows ;i++)
		//	{
		//		p = frame.ptr<uchar>(i);
		//		for(j = 0;j < col;j+=3)
		//		{
		//			buffer[i * col + j] = p[j];
		//			buffer[i * col + j + 1] = p[j+1];
		//			buffer[i * col + j + 2] = p[j+2];
		//			//if(j %27 < 9)
		//			//{
		//			//buffer[i * col + j] = 0xfd;
		//			//buffer[i * col + j + 1] = (uchar)i;
		//			//buffer[i * col + j + 2] = j%27;
		//			//}
		//			//else if(j %27 < 18)
		//			//{
		//			//buffer[i * col + j] = (uchar)i;
		//			//buffer[i * col + j + 1] = 0xfe;
		//			//buffer[i * col + j + 2] = j%27;
		//			//}
		//			//else if(j %27 < 27)
		//			//{
		//			//buffer[i * col + j] = (uchar)i;
		//			//buffer[i * col+ j + 1] = j%27;
		//			//buffer[i * col + j + 2] = 0xff;
		//			//}
		//		}
		//	}
		//	
		//}

}
void Leah::calDetect(uchar* buffer, int size)
{
	Mat frame,mask;//Camera frame container
	Mat referance = imread("referance.png");//Load histogram referance image
#pragma region Historgram Creation
	cvtColor(referance,referance,CV_BGR2HSV);
	MatND hist;
	int h_bins = 30; int s_bins = 32;
	int histSize[] = { h_bins, s_bins };
	float h_range[] = { 0, 179 };
	float s_range[] = { 0, 255 };
	const float* ranges[] = { h_range, s_range };
	int channels[] = { 0, 1 };
	calcHist( &referance, 1, channels, Mat(), hist, 2, histSize, ranges, true, false );
	normalize( hist, hist, 0, 255, NORM_MINMAX, -1, Mat() );
#pragma endregion

	vector<vector<Point> > contours,contoursTwo;
	vector<Vec4i> hierarchy;
	int64 start,stop;
	double time;
		start = getTickCount();
		Mat target,passed;
		*capture >> frame;//Capturea frame from the cmaera and place it in frame
		pyrDown(frame,mask,Size(frame.cols / 2,frame.rows /2));//Scale down the sourcei mage by a factor of 4
		pyrDown(mask,mask,Size(mask.cols / 2,mask.rows /2));//This is done to reduce noise and increase framerate

		cvtColor(mask,mask,CV_BGR2HSV);//Color space conversion for more accurate color detection

		calcBackProject( &mask, 1, channels, hist, passed, ranges, 2, true );
		blur(passed,passed,Size(5,5));
		threshold(passed,passed,THRESHOLD ,255,THRESH_BINARY);
		Canny(passed,passed,30,90);
		int dilation_size = 5;
		Mat element = getStructuringElement( MORPH_RECT,
                                Size( 2*dilation_size + 1, 2*dilation_size+1 ),
                                Point( dilation_size, dilation_size ) );
		dilate(passed,passed,element);
		findContours( passed, contours, hierarchy, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_SIMPLE, Point(0, 0) );
		vector<vector<Point> > contours_poly( contours.size() );
		vector<Rect> boxes( contours.size() );
		for( int j = 0; j < contours.size(); j++ )
		{ 
			approxPolyDP( Mat(contours[j]), contours_poly[j], 3, true );
			boxes[j] = boundingRect( Mat(contours_poly[j]) );

			rectangle(frame,boxes[j].tl() * 4,boxes[j].br() * 4,Scalar(0,0,255),2,8,0);

		}
		//Incrament the frame counter. If the target is found the count will be zeroed anyways
		CurrentTarget->FramesSinceLast++;

		//Test if we can do a second pass
		for(int i = 0; i < boxes.size();i++)
		{
			if(boxes[i].area() > 625)
			{
				cout << "Entering second pass big box: " << i << '\n';
				Mat passTwo,maskTwo,sourceTwo;
				vector<Vec4i> hierarchyTwo;
				Rect test = Rect(boxes[i].tl() * 4,boxes[i].br() *4);
				frame(test).copyTo(sourceTwo);
				
				cvtColor(sourceTwo,maskTwo,CV_BGR2HSV);//Color space conversion for more accurate color detection
				blur(maskTwo,maskTwo,Size(5,5));
				calcBackProject( &maskTwo, 1, channels, hist, passTwo, ranges, 2, true );
				threshold(passTwo,passTwo,THRESHOLD ,255,THRESH_BINARY);
				dilate(passTwo,passTwo,element);
				//imshow("passTwo",passTwo);
				findContours( passTwo, contoursTwo, hierarchyTwo, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_SIMPLE, Point(0, 0) );
				vector<vector<Point> > contours_poly( contoursTwo.size() );
				vector<Rect> boxesTwo( contoursTwo.size() );
				//vector<Point2f>center( contoursTwo.size() );
				//vector<float>radius( contoursTwo.size() );
				vector<double>area (contoursTwo.size() );
				//cout << "Number of shapes: "<< contoursTwo.size() <<'\n';
				for( int j = 0; j < contoursTwo.size(); j++ )
				{ 
					//cout << "Pass " << j << '\n';
					approxPolyDP( Mat(contoursTwo[j]), contours_poly[j], 3, true );
					boxesTwo[j] = boundingRect( Mat(contours_poly[j]) );
					area[j] = contourArea(contoursTwo[j]);
					rectangle(frame,boxesTwo[j].tl() ,boxesTwo[j].br() ,Scalar(0,255,0),2,8,0);
					//minEnclosingCircle( (Mat)contours_poly[j], center[j], radius[j] );
					
				}
				for( int j = 0; j < boxesTwo.size(); j++ )
				{
					//cout << "Circle test \n"; 
					//float cArea = (3.14 * radius[j] * radius[j]);
					if((area[j] * 0.785 > boxesTwo[j].area() - boxesTwo[j].area()*0.40 ) )//&& (area[j] * 0.785 < cArea + cArea*0.10 ))//0.40 /*boxesTwo[j].area()*/
					{
						//cout << "Circle found \n";
						//circle( sourceTwo, Point(center[j]) , (int)radius[j] , Scalar(255,0,0), 2, 8, 0 );
						putText(frame,"Target",boxesTwo[j].tl()  + boxes[i].tl() * 4,0,1,Scalar(255,0,0),2,8,false);
						CurrentTarget->FramesSinceLast = 0;
						CurrentTarget->valid = true;
						CurrentTarget->tlx = boxesTwo[j].tl().x;
						CurrentTarget->tly = boxesTwo[j].tl().y;
						CurrentTarget->brx = boxesTwo[j].br().x;
						CurrentTarget->bry = boxesTwo[j].br().y;
					}
				}
				//imshow("maskTwo",sourceTwo);
			}
		}
		
//Loop delay block

		stop = getTickCount();
		time = (stop - start)/getTickFrequency();
		//putText(frame,std::to_string(time),Point(100,100),0,1,Scalar(255,255,255),2,8,false);
		putText(frame,std::to_string(1/time),Point(100,200),0,1,Scalar(255,255,155),2,8,false);
		//imshow("Input",frame);
		//imshow("passed",passed);
		//cvtColor(frame,frame,CV_BGR2GRAY);

		//waitKey(5);
		if(size >= frame.rows * frame.cols * 3)
		{
			for(int i = 0; i < frame.rows * frame.cols *3;i++)
			{
				buffer[i] = frame.data[i];
			}
			
		}
		//return data;
}

void Leah::Mapping(int* data,int size,int angle, float bearing, float x,float y)
{
	Mat NewFrame = Mat::zeros(MaxRange * 2 * SCALE,MaxRange * 2 * SCALE,CV_8UC1);
	float slitAngle = (float)angle / (float)size;
	for(int i = 0; i < size;i++)
	{
		if(data[i] < MaxRange)
		{
			int X = (int)((data[i] * SCALE * sin(((i * slitAngle) - 45 + 90) * (PIE / 180))) + MaxRange * SCALE);
			int Y = (int)((data[i] * SCALE * cos(((i * slitAngle) - 45 + 90) * (PIE / 180))) + MaxRange * SCALE);
			if( X >= 2*  MaxRange * SCALE - 1)
			{
				X = MaxRange * SCALE - 1;
			}
			else if(X <= 0)
			{
				X =0;
			}
			if( Y >= 2* MaxRange * SCALE - 1)
			{
				Y = MaxRange * SCALE - 1;
			}
			else if(Y <= 0)
			{
				Y = 0;
			}
			NewFrame.at<uchar>(X,Y) = 0xFF;
		}
	}
	
	int dilation_size = 5;
	Mat element = getStructuringElement( MORPH_RECT,
							Size( 2*dilation_size + 1, 2*dilation_size+1 ),
							Point( dilation_size, dilation_size ) );
	dilate(NewFrame,NewFrame,element);
	vector<vector<Point>> contours;
	vector<Vec4i> hierarchy;
	findContours( NewFrame,contours,hierarchy,CV_RETR_TREE,CV_CHAIN_APPROX_SIMPLE, Point(0,0));

	vector<RotatedRect> minRect( contours.size());
	for(int i = 0; i < contours.size();i++)
	{
		minRect[i] = minAreaRect(Mat(contours[i]));
	}
	for(int i = 0; i< contours.size(); i++)
	{
		Point rect_points[1][4];
		Point2f temp[4];
		minRect[i].points(temp);
		for(int j = 0; j < 4; j++)
		{
			rect_points[0][j] = temp[j];
		}
		const Point* ppt[1] = {rect_points[0] };
		int npt[] = {4 };
		fillPoly(NewFrame,ppt,npt,1,Scalar(50),8);
	}
	imshow("Raw Data", NewFrame);
	//Point center = Point(MaxRange * SCALE, MaxRange * SCALE);
	//Mat rot_mat = getRotationMatrix2D(center,bearing,1);
	//warpAffine( NewFrame,NewFrame,rot_mat,NewFrame.size());
	
	//combine(NewFrame,x,y);

	
	waitKey(5);
}


void Leah::combine(Mat input,int xp, int yp)
{
	for(int y = 0;y < input.rows;y++)
	{
		for(int x = 0; x < input.cols;x++)
		{
			int address =  y * input.cols + x;
			if(input.data[address] > 0)
			{
				if(CollisionMap->data[(y + yp)* CollisionMap->cols + x + xp] + input.data[address] >= 255)
				{
					CollisionMap->data[(y + yp)* CollisionMap->cols + x + xp] = 0xff;
				}
				else
					CollisionMap->data[(y + yp)* CollisionMap->cols + x + xp] += input.data[address];
			}
			else
			{
				CollisionMap->data[(y + yp)* CollisionMap->cols + x + xp] = 0;
			}
		}
	}
}

Target* Leah::GetTarget()
{
	return CurrentTarget;
}

