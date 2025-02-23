import carteVisite from "../../assets/home/carte_visite.png"
import devanture from "../../assets/home/devanture.png"
import goodies from "../../assets/home/goodies.png"
import easySaveCreator from "../../assets/home/easySaveCreator.png"
import HomePageElement from "../../components/HomePageElement/HomePageElement"
import { useState, useEffect } from "react"
import { Fade } from "../../animations/Fade"
import "./Home.css"


function setVisible(className, setVisible, entry) {
  console.log(entry.isIntersecting)
  if (entry.target.className === className) {
    if (entry.isIntersecting) {
      setVisible(true);
    }
    else {
      setVisible(false);
    }
  }
}
export default function Home() {
  const [tiktokCreator, setTiktokCreator] = useState(true);
  const [produitVisible, setProduitVisible] = useState(false);
  const [carteVisible, setCarteVisible] = useState(false);
  const [bureauxVisible, setBureauxVisible] = useState(false);

  useEffect(() => {
    window.scrollTo(0, 0);
    const observer = new IntersectionObserver(
      ([entry]) => {
        setVisible("element-produit", setProduitVisible, entry);
        setVisible("element-carte", setCarteVisible, entry);
        setVisible("element-bureaux", setBureauxVisible, entry);
        setVisible("element-tiktokCreator", setTiktokCreator, entry);
      },
      {
        rootMargin: "-120px",
      } // Ajustez cette valeur selon la hauteur de votre HomeBody
    );

    const elements = [
      document.querySelector('.element-produit'),
      document.querySelector('.element-carte'),
      document.querySelector('.element-bureaux'),
      document.querySelector('.element-tiktokCreator')
    ];

    elements.forEach(element => {
      if (element) {
        observer.observe(element);
      }
    });
    return () => observer.disconnect();
  }, []);


  return (
    <div className="Home">
      <div className="element-tiktokCreator">
        <Fade visible={tiktokCreator}>
          <HomePageElement
            title={"EasySave"}
            description="EasySave est une application avancée qui facilite la sauvegarde de vos données en toute sécurité. Elle propose des sauvegardes incrémentielles et complètes, permettant d’optimiser l’espace et le temps de stockage. L’utilisateur peut choisir l’emplacement de sauvegarde : disque local, serveur distant ou cloud, garantissant ainsi une flexibilité totale. Grâce à son interface intuitive et à ses algorithmes intelligents, EasySave assure une protection efficace des fichiers contre les pertes accidentelles. Ce projet révolutionne la gestion des sauvegardes en combinant automatisation, sécurité et personnalisation pour une expérience fluide et fiable."
            img={easySaveCreator}
          />
        </Fade>
      </div>
      <Fade visible={produitVisible}>
        <div className="element-produit">
          <HomePageElement
            title="Produit dérivée"
            description="Les solutions de sauvegarde intelligentes sont un élément essentiel de la protection des données numériques. Elles incluent des options variées, telles que la sauvegarde automatique, la synchronisation en temps réel, et la planification personnalisée pour s’adapter aux besoins des utilisateurs. Ces solutions permettent de stocker les fichiers sur différents supports, qu’il s’agisse d’un disque dur local, d’un serveur distant, d’un NAS ou d’un service cloud sécurisé. Grâce à des technologies avancées comme le chiffrement des données et la compression optimisée, elles garantissent une protection efficace tout en minimisant l’espace de stockage utilisé. Que ce soit pour un usage personnel ou professionnel, ces outils assurent la sécurité et l’accessibilité des données en toute simplicité."
            img={goodies}
            inversion={true}
          />
        </div>
      </Fade>
      <Fade visible={carteVisible}>
        <div className="element-carte">
          <HomePageElement
            className="element-carte"
            title="Visibilité"
            description="La communication et la visibilité autour de EasySave mettent en avant ses fonctionnalités avancées tout en illustrant concrètement ses avantages avec des démonstrations et des cas d’usage. Le ton est rassurant et pédagogique, axé sur la sécurité et la simplicité d’utilisation pour attirer un large public. La publication de contenu est régulière et variée, incluant des tutoriels expliquant comment optimiser ses sauvegardes, des témoignages d’utilisateurs satisfaits, et des alertes sur les bonnes pratiques en cybersécurité. L’objectif est de bâtir une communauté engagée autour de la protection des données, tout en soulignant l’importance d’une gestion proactive des sauvegardes."
            img={carteVisite}
          />
        </div>
      </Fade>
      <Fade visible={bureauxVisible}>
        <div className="element-bureaux">
          <HomePageElement
            title="Nos bureaux"
            description="Les bureaux de EasySave offrent un environnement moderne et sécurisé, idéal pour le développement de solutions de sauvegarde avancées. L’ambiance de travail est à la fois collaborative et axée sur l’innovation, avec une équipe passionnée par la protection et l’optimisation des données. Les employés évoluent dans un espace technologique de pointe, équipé d’infrastructures performantes garantissant efficacité et confort. L’atmosphère est à la fois professionnelle et conviviale, encourageant le partage des idées et l’amélioration continue des fonctionnalités pour répondre aux besoins des utilisateurs."
            img={devanture}
            inversion={true}
          />
        </div>
      </Fade>
    </div>
  );
}