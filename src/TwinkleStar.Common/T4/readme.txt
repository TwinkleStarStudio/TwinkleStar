安装T4 Toolbox插件用于多文本的生成
安装Devart T4 Editor For Visual Studio插件用于模版代码提示

使用时 把T4Model文件夹拷贝到项目中
替换相应的命名空间以及dll
取消注释

如果models是按模块划分的用 T4ModelInfo model = new T4ModelInfo(modelType, true);
反之 T4ModelInfo model = new T4ModelInfo(modelType);