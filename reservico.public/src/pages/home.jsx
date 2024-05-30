import Banner from "@/components/home/banner"
import InfoCards from "@/components/home/info-cards"
import HowItWorks from "@/components/home/how-it-works"
import GotQuestions from "@/components/home/got-questions"

const Home = () => {
  return (    
    <div>      
      <Banner />
      <InfoCards />
      <HowItWorks />
      <GotQuestions />
    </div>
  )
}

export default Home