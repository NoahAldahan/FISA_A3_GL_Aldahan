import "./HomePageElement.css"

export default function HomePageElement({ title = "", description = "", img, alt = "pas d'image", inversion = false }) {
    if (inversion) {
        return (<header className="element">
            <div className="element-right">
                <img src={img} alt={alt} class="element-img"></img>
            </div>
            <div className="element-left">
                <h1 className="element-title">{title}</h1>
                <p className="element-description">{description}</p>
            </div>
        </header>);
    }
    else {
        return (
            <header className="element">
                <div className="element-left">
                    <h1 className="element-title">{title}</h1>
                    <p className="element-description">{description}</p>
                </div>
                <div className="element-right">
                    <img src={img} alt={alt} class="element-img"></img>
                </div>
            </header>
        );
    }
}