using RDBtoJSON.Extractors;

namespace RDBtoJSON.ParamControls
{
    public interface IParamsExtractor
	{
        IDataExtractor Extractor { get; }
    }
}
