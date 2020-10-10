# GzeNewCrawler  
对SearchTest进行了重新设计，增加了部分功能。
## 使用方式  
1.创建一个Crawler类的子类  
2.实现PageProcessor方法  
3.创建一个Crawler子类的对象，并调用setThreadNum设置线程数量  
调用setRetryTime设置网页无法打开时的重试次数  
调用setDepth设置爬虫打开网页的深度  
4.调用run方法启动爬虫，其中参数url表示爬虫第一个打开的网页
